using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(BoxCollider2D))]
[SelectionBase]
public class SpawnPoint : MonoBehaviour {

    /*
    private SpriteRenderer _SpriteRenderer = null;
    private SpriteRenderer spriteRendrer
    {
        get
        {
            if(_SpriteRenderer == null)
            {
                _SpriteRenderer = GetComponent<SpriteRenderer>();
            }
            return _SpriteRenderer;
        }
    }
    private Text _textUi;
    private Text textUi
    {
        get
        {
            if(_textUi == null)
            {
                _textUi = GetComponentInChildren<Text>();
            }
            return _textUi;
        }
    }
    */
    private BoxCollider2D _BoxCollider = null;
    private BoxCollider2D boxCollider
    {
        get
        {
            if (_BoxCollider == null)
            {
                _BoxCollider = GetComponent<BoxCollider2D>();
            }
            return _BoxCollider;
        }
    }

    protected string labelText;
    public Color gizmosColor = Color.white;

    protected virtual void Awake()
    {
        //_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void SetColor(Color color)
    {
        //color.a = 0.4f;
        //spriteRendrer.color = color;
    }

    protected void SetLabel(string text)
    {
        //textUi.text = text;
    }

    public GameObject Spawn(ObjectPool pool)
    {
        GameObject instance = pool.create().gameObject;
        instance.transform.position = GetSpawnPosition();
        return instance;
    }

    private Vector3 GetSpawnPosition()
    {
        var min = boxCollider.bounds.min;
        var max = boxCollider.bounds.max;
        Vector2 pos = new Vector2();
        pos.x = Random.Range(min.x, max.x);
        pos.y = Random.Range(min.y, max.y);
        return pos;
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = gizmosColor;
        Gizmos.DrawCube(transform.position, boxCollider.size);
        //Gizmos.DrawWireCube(transform.position, spriteRendrer.size);

        Handles.Label(transform.position, labelText);

    }

}
