using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script gives users a simple Object Pool Manager (singleton) to handle GameObjects in Unity.<br/><br/>
/// <b>Required GameObjects:</b>
/// <list type="bullet">
///    <item>
///        <term> Object prefab </term>
///        <description> The object that will be instantiated (e.g. an enemy or a bullet). </description>
///    </item>
///    <item>
///        <term> Object Container </term>
///        <description> The parent object holding your GOs. Useful to keep the Hierarchy organized. </description>
///    </item>
/// </list>
/// <br/>
/// Of course feel free to contribute :) <br/>
/// <br/>
/// </summary>
/// <typeparam name="T"></typeparam>
/// 
/// ------------------------------------------
/// By: otreblA (aka otreblA_SNAKE_[ITA])
/// ------------------------------------------
/// 

public abstract class ObjectPoolManager<T> : MonoBehaviour where T : MonoBehaviour
{
    ///<example>
    ///   <code>
    ///   public override void AttackPlayer()
    ///   {
    ///   Depending on your implementation it can be used in different ways:
    ///   EITHER THIS->Projectile bullet = BulletPoolManager.Instance.RequestGameObject();
    ///   OR THIS->Projectile bullet = ObjectPoolManager<Projectile>.Instance.RequestGameObject();
    ///   bullet.transform.position = muzzle.transform.position + transform.forward;
    ///   bullet.transform.rotation = this.transform.rotation;
    ///   bullet.teamOfBullet = TeamEnum.EnemySide;
    ///   bullet.DamageAmount = damageToInflict;
    ///   }
    ///   </code>
    ///</example>

    public static ObjectPoolManager<T> Instance { get; private set; }

    [SerializeField]
    private GameObject _objectPrefab;

    [SerializeField]
    private GameObject _objectsContainer;

    private List<GameObject> _objectPool;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        _objectPool = PoolGenerator(10);
    }

    public T RequestGameObject()
    {
        foreach (GameObject obj in _objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj.GetComponent<T>();
            }
        }

        return CreateNewInstance();
    }

    private List<GameObject> PoolGenerator(int quantity)
    {
        _objectPool = new List<GameObject>(quantity);

        int i = 0;

        while (i < quantity)
        {
            CreateNewInstance();
            i++;
        }

        return _objectPool;
    }

    private T CreateNewInstance()
    {
        GameObject obj = Instantiate(_objectPrefab);
        obj.transform.parent = _objectsContainer.transform;
        obj.SetActive(false);
        _objectPool.Add(obj);

        return obj.GetComponent<T>();
    }
}