using System;
using UnityEngine;
using ODProjects.PoolModule.Data.ScriptableObjects;
using ODProjects.PoolModule.Enums;
using ODProjects.PoolModule.Extentions;
using ODProjects.PoolModule.Interfaces;
using ODProjects.PoolModule.Signals;

namespace ODProjects.PoolModule
{
    public class PoolManager : MonoBehaviour, IGetPoolObject, IReleasePoolObject
    {
        #region FIELDS

        #region Serialize Fields

        [SerializeField] private PoolSignals poolSignals;
        [SerializeField] private CD_Pool data;
        #endregion
        
        #region Private Fields
        private PoolType _listCountCache;
        private ObjectPoolExtention _extention;
        #endregion

        #endregion

        #region FUNCTIONS

        #region Initialization

        private void Awake()
        {
            _extention = new ObjectPoolExtention();
            InitializePools();
            
            Debug.Log("Pool Manager Initialized!");
        }
        
        private void InitializePools()
        {
            foreach (PoolType value in Enum.GetValues(typeof(PoolType)))
            {
                if(!data.HasThisType(value)) return;
                
                _listCountCache = value;
                InitPool(value, data.PoolDataDictionary[value].initalAmount, data.PoolDataDictionary[value].isDynamic);
            }
        }

        private void InitPool(PoolType poolType, int initalAmount, bool isDynamic)
        {
            _extention.AddObjectPool<GameObject>(FactoryMethod, TurnOnObject, TurnOffObject, poolType, initalAmount, isDynamic);
        }


        #endregion
        
        #region Get & Release Functions

        public GameObject OnGetObjectFromPool(PoolType poolType)
        {
            _listCountCache = poolType;
            return _extention.GetObject<GameObject>(poolType);
        }
        public void OnReleaseObjectFromPool(GameObject obj, PoolType poolType)
        {
            _listCountCache = poolType;
            ResetObject(obj);
            _extention.ReturnObject<GameObject>(obj, poolType);
        }
        
        #endregion
        
        #region Auxilary tools
        
        private void ResetObject(GameObject obj)
        {
            obj.transform.parent = this.transform;
        }
        
        private void TurnOnObject(GameObject obj)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        private void TurnOffObject(GameObject obj)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        private GameObject FactoryMethod()
        {
            var go = Instantiate(data.PoolDataDictionary[_listCountCache].ObjectType, this.transform);
            Debug.Log($"{_listCountCache} Object Instantiated !!!");
            return go;
        }

        #endregion
        
        #region Event Subscription
        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            poolSignals.OnGetObjectFromPool += OnGetObjectFromPool;
            poolSignals.OnReleaseObjectFromPool += OnReleaseObjectFromPool;
            Debug.Log("Subscribed to Pool Events");
        }
        private void UnsubscribeEvents()
        {
            poolSignals.OnGetObjectFromPool -= OnGetObjectFromPool;
            poolSignals.OnReleaseObjectFromPool -= OnReleaseObjectFromPool;
            Debug.Log("Unsubscribed to Pool Events");
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        #endregion
        
    }
}