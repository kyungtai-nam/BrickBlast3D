using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;
using VoxelBusters.NativePlugins.Internal;

public class RcBilling : GSingletonMono<RcBilling>
{
	public enum E_BILLING_RESULT
	{
		Cancel,
		Success,
		Failed,
		Already,
		Total
	}

	public delegate void funcInit(bool result);
	public delegate void funcBuy(E_BILLING_RESULT eResult, BillingTransaction billing);

	private funcInit								m_OnCallbackInit			= null;
	private funcBuy									m_OnCallbackBuy				= null;
	private Dictionary<string, BillingProduct>		m_hashProducts				= new Dictionary<string, BillingProduct>();
	private Dictionary<string, BillingTransaction>	m_hashHasProducts			= new Dictionary<string, BillingTransaction>();
	private	bool									m_productRequestFinished	= false;
	private float 									m_fTimeOut					= 0f;
	private bool 									m_IsWaitRequest				= false;

	public bool productRequestFinished
	{
		get { return m_productRequestFinished; }
	}

	public bool available
	{
		get { return NPBinding.Billing.IsAvailable(); }
	}

	// Use this for initialization
	protected override void Awake()
	{
		base.Awake();

		m_productRequestFinished = false;
	}

	void OnEnable()
	{
		Billing.DidFinishRequestForBillingProductsEvent += OnDidFinishProductsRequest;
		Billing.DidFinishProductPurchaseEvent	        += OnDidFinishTransaction;
		Billing.DidFinishRestoringPurchasesEvent		+= OnDidFinishRestoringPurchases;
	}

	void OnDisable()
	{
		this.CancelInvoke();

		Billing.DidFinishRequestForBillingProductsEvent	-= OnDidFinishProductsRequest;
		Billing.DidFinishProductPurchaseEvent	        -= OnDidFinishTransaction;
		Billing.DidFinishRestoringPurchasesEvent		-= OnDidFinishRestoringPurchases;
	}

	public void Reset()
	{
		Billing billing = NPBinding.Instance.GetComponent<Billing>();
		if( null != billing )
		{
			#if DEBUG_MODE
			Debug.Log("RcBilling:Reset Billing="+billing);
			#endif
			GameObject.Destroy(billing);
		}
		m_IsWaitRequest 			= false;
		m_productRequestFinished 	= false;
	}

	public void Init(funcInit OnCallback, float fTimeOut=3f)
	{
		// 초기화.

		#if DEBUG_MODE
		Debug.Log("RcBilling:Init");
		#endif

		if( true == m_IsWaitRequest )
			return;

		m_IsWaitRequest		= true;
		m_fTimeOut			= fTimeOut;
		m_OnCallbackInit 	= OnCallback;
				
		if( true == this.available )
			_RequestBillingProducts(NPSettings.Billing.Products);
		else
		{
			if( null != m_OnCallbackInit )
				m_OnCallbackInit(false);
			m_IsWaitRequest = false;
		}
	}

	public BillingProduct GetProduct(string strIdentifier)
	{
		// 제품 정보 리턴.

		//#if DEBUG_MODE
		//Debug.Log("RcBilling:GetProduct="+strIdentifier);
		//#endif
		
		BillingProduct product = null;
		if( true == m_hashProducts.TryGetValue(strIdentifier, out product) )
			return product;
		return null;
	}

	public bool Buy(string strIdentifier, funcBuy OnCallback)
	{
		// 구입.
		#if DEBUG_MODE
		Debug.Log("RcBilling:Buy="+strIdentifier);
		#endif

		m_OnCallbackBuy = OnCallback;

		if( true == this.available )
		{
			BillingProduct product = GetProduct(strIdentifier);
			if( null != product )
			{
				// 비소모 아이템 이면서 
				if( false == product.IsConsumable )
				{
					// 이미 구입했다면
					if( true == _IsProductPurchased(product) )
					{
						// 이미 구입 콜백 
						if( null != m_OnCallbackBuy )
							m_OnCallbackBuy(E_BILLING_RESULT.Already, null);
						return false;
					}
				}

				_BuyProduct(product);
				return true;
			}
		}

		if( null != m_OnCallbackBuy )
			m_OnCallbackBuy(E_BILLING_RESULT.Failed, null);

		return false;
	}

	public bool HasProduct(string strIdentifier)
	{
		// 이미 구입한 상품인지 검사.

		bool ret = false;
		string errMsg = "";

		BillingTransaction billing = null;
		if( true == m_hashHasProducts.TryGetValue(strIdentifier, out billing) )
		{
			if (string.IsNullOrEmpty (billing.Error))
				ret = true;
			else
				errMsg = billing.Error;
		}

		#if DEBUG_MODE
		Debug.Log(string.Format("RcBilling:HasProduct={0} ret={1} err={2}", strIdentifier, ret, errMsg));
		#endif

		return ret;
	}

	public void RestorePurchases(funcBuy OnCallback)
	{
		// 상품 복원.

		if( 0 == m_hashHasProducts.Count )
			return;

		foreach( KeyValuePair<string, BillingTransaction> it in m_hashHasProducts )
		{
			if( eBillingTransactionState.RESTORED == it.Value.TransactionState )
				m_OnCallbackBuy(E_BILLING_RESULT.Success, it.Value);
		}
		m_hashHasProducts.Clear();
	}



#region API Methods
	private void _RequestBillingProducts(BillingProduct[]  _products)
	{
		#if DEBUG_MODE
		for(int i=0; i<_products.Length; ++i)
			Debug.Log("RcBilling:_RequestBillingProducts="+_products[i].ToString());
		#endif

		this.CancelInvoke();
		if( 0f < m_fTimeOut )
			this.Invoke("OnRequestBillingProductsTimeOut", m_fTimeOut);
		NPBinding.Billing.RequestForBillingProducts(_products);
	}

	private void _BuyProduct(BillingProduct _product)
	{
		#if DEBUG_MODE
		Debug.Log("RcBilling:_BuyProduct="+_product);
		#endif

		NPBinding.Billing.BuyProduct(_product);
	}

	// 이미 구입했던 아이템인가? (비소모성)
	private bool _IsProductPurchased(BillingProduct _product)
	{		
		bool ret = NPBinding.Billing.IsProductPurchased(_product);

		#if DEBUG_MODE
		Debug.Log(string.Format("RcBilling:_IsProductPurchased={0} ret={1}", _product, ret));
		#endif

		return ret;
	}
#endregion


#region API Callback Methods
	void OnDidFinishProductsRequest(BillingProduct[] _regProductsList, string _error)
	{
		#if DEBUG_MODE
		Debug.Log("RcBilling:OnDidFinishProductsRequest="+_regProductsList+" error="+_error);
		#endif
		
		this.CancelInvoke();
		m_IsWaitRequest = false;

		m_hashProducts.Clear();
		if( !string.IsNullOrEmpty(_error) )
		{
			m_productRequestFinished = false;

			if( null != m_OnCallbackInit )
				m_OnCallbackInit(false);
		}
		else
		{
			m_productRequestFinished = true;
			foreach( BillingProduct it in _regProductsList )
			{
				m_hashProducts.Add(it.ProductIdentifier, it);
			}

			if( null != m_OnCallbackInit )
				m_OnCallbackInit(true);

#if UNITY_ANDROID
			NPBinding.Billing.RestorePurchases();
#endif
		}
	}

	void OnDidFinishTransaction(BillingTransaction _transaction)
	{
		#if DEBUG_MODE
		Debug.Log("RcBilling:OnDidFinishTransaction="+_transaction+" error="+_transaction.Error.GetPrintableString());
		#endif

		/*

		Product Identifier 		= com.flerogames.cl.flyinglarva.donut1
		Transaction Date[UTC] 	= 07/05/2016 14:47:27
		Transaction Identifier 	= ffkihfoeobikcigcfbgmhbcf.AO-J1OyKIfFnwvBUJH3NE67RSvpEbUwOQ34L3ntrNP_6BwP9nYQjFB0j5TGVugRbFXPRegBVHFgrnJFBkKJK94ajjflyQ6T9EB40kJmoLchVr7z9N7pz_J6-aVtJq6H_vBBaSNZWvOIivnlBkLgOjz5-FeOOZnLUGufM4Rc0iZRl50uFELtjx9I
		Transaction Receipt 	= BBM7LYh1gIgBcn4yAhIEBv3gAj8ifME7XrEiflB27zuoraXZVzcK2X65kNrEspEHEMUeGqnMQeN8r/xWhc+Y4vYDM9ACGc82Nuwi2HQPQK/gkeW1HUlIeR+XI4gr2AMxIxCsPjRU/IimXON+rysAlkUFl/KLl1SF1v4bPbgKbIi4fLG6XIcp9uqFD8WrrFXBw6TxH7RWml1u7LBXq+PWe2wU1yb1EZl9CLbgxW1LljhdZ3PBkd2HWs1/UBTqtk8H6JwAtll1Tvr581eE1cvyLg6VfXdQQ7ZTnBFUy0DU6PtwxoiqxNS5yZyG/PFW4ncAy+kBtxG9kvbz2dUxfW+rqg==

		{
			"packageName":"com.flerogames.cl.flyinglarva",
			"productId":"com.flerogames.cl.flyinglarva.donut1",
			"purchaseTime":1467730047159,
			"purchaseState":0,
			"purchaseToken":"ffkihfoeobikcigcfbgmhbcf.AO-J1OyKIfFnwvBUJH3NE67RSvpEbUwOQ34L3ntrNP_6BwP9nYQjFB0j5TGVugRbFXPRegBVHFgrnJFBkKJK94ajjflyQ6T9EB40kJmoLchVr7z9N7pz_J6-aVtJq6H_vBBaSNZWvOIivnlBkLgOjz5-FeOOZnLUGufM4Rc0iZRl50uFELtjx9I"
		}
		
		*/

		if( null == _transaction )
		{
			if( null != m_OnCallbackBuy )
				m_OnCallbackBuy(E_BILLING_RESULT.Failed, null);
		}
		else
		{
			#if DEBUG_MODE
			Debug.Log("Product Identifier = " 		+ _transaction.ProductIdentifier);
			Debug.Log("Transaction State = "		+ _transaction.TransactionState);
			Debug.Log("Verification State = "		+ _transaction.VerificationState);
			Debug.Log("Transaction Date[UTC] = "	+ _transaction.TransactionDateUTC);
			Debug.Log("Transaction Date[Local] = "	+ _transaction.TransactionDateLocal);
			Debug.Log("Transaction Identifier = "	+ _transaction.TransactionIdentifier);
			Debug.Log("Transaction Receipt = "		+ _transaction.TransactionReceipt);
			Debug.Log("Error = "					+ _transaction.Error.GetPrintableString());
			Debug.Log("RawPurchaseData = "			+ _transaction.RawPurchaseData);
			#endif
						
			if( !string.IsNullOrEmpty(_transaction.Error) )
			{
#if UNITY_EDITOR
				if( _transaction.Error.Contains("cancelled") )
#else
				if( _transaction.Error.Contains("-1005") )
#endif
				{
					if( null != m_OnCallbackBuy )
						m_OnCallbackBuy(E_BILLING_RESULT.Cancel, _transaction);
					return;
				}
			}

			switch(_transaction.VerificationState)
			{
			case eBillingTransactionVerificationState.NOT_CHECKED:
			case eBillingTransactionVerificationState.SUCCESS:
				if( eBillingTransactionState.PURCHASED == _transaction.TransactionState )
				{
					if( null != m_OnCallbackBuy )
						m_OnCallbackBuy(E_BILLING_RESULT.Success, _transaction);
				}
				else
				{
					if( null != m_OnCallbackBuy )
						m_OnCallbackBuy(E_BILLING_RESULT.Failed, _transaction);
				}
				break;

			case eBillingTransactionVerificationState.FAILED:
				if( null != m_OnCallbackBuy )
					m_OnCallbackBuy(E_BILLING_RESULT.Failed, _transaction);
				break;
			}
		}
	}

	void OnDidFinishRestoringPurchases(BillingTransaction[] _transactions, string _error)
	{
		#if DEBUG_MODE
		Debug.Log(string.Format("RcBilling:OnDidFinishRestoringPurchases error={0}.", _error.GetPrintableString()));
		#endif
	
		m_hashHasProducts.Clear();
		if( !string.IsNullOrEmpty(_error) )
		{
			#if DEBUG_MODE
			Debug.LogError("RcBilling:OnDidFinishRestoringPurchases error="+_error);
			#endif
		}
		else
		{
			if( null != _transactions )
			{
				#if DEBUG_MODE
				Debug.Log(string.Format("Count of transaction information received = {0}.", _transactions.Length));
				#endif
				
				foreach( BillingTransaction it in _transactions )
				{
					#if DEBUG_MODE
					Debug.Log("Product Identifier = " 		+ it.ProductIdentifier);
					Debug.Log("Transaction State = "		+ it.TransactionState);
					Debug.Log("Verification State = "		+ it.VerificationState);
					Debug.Log("Transaction Date[UTC] = "	+ it.TransactionDateUTC);
					Debug.Log("Transaction Date[Local] = "	+ it.TransactionDateLocal);
					Debug.Log("Transaction Identifier = "	+ it.TransactionIdentifier);
					Debug.Log("Transaction Receipt = "		+ it.TransactionReceipt);
					Debug.Log("Error = "					+ it.Error.GetPrintableString());
					#endif

					m_hashHasProducts.Add(it.ProductIdentifier, it);
				}
			}
		}
	}

	void OnRequestBillingProductsTimeOut()
	{
		#if DEBUG_MODE
		Debug.Log("RcBilling:OnRequestBillingProductsTimeOut");
		#endif
		
		m_IsWaitRequest = false;
		if( null != m_OnCallbackInit )
			m_OnCallbackInit(false);
	}
#endregion

#region Callback Example
	void OnBillingInitCB(bool result)
	{
		// TODO: 인앱 상품 정보 설정.
	}

	void OnBillingBuyCB(E_BILLING_RESULT eResult, BillingTransaction billing)
	{
		switch(eResult)
		{
		case E_BILLING_RESULT.Cancel:
			// 취소.
			break;

		case E_BILLING_RESULT.Success:
			// 성공.
			break;

		case E_BILLING_RESULT.Failed:
			// 실패.
			break;

		case E_BILLING_RESULT.Already:
			// 이미 구입
			break;
		}
	}
#endregion
}

