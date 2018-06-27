using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(SpriteRenderer))]
[SelectionBase]
public class SpawnPoint : MonoBehaviour {
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

    protected virtual void Awake()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void SetColor(Color color)
    {
        color.a = 0.4f;
        spriteRendrer.color = color;
    }

    protected void SetLabel(string text)
    {
        textUi.text = text;
    }

    public GameObject Spawn(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
        return instance;
    }

    private Vector3 GetSpawnPosition()
    {
        var min = _SpriteRenderer.bounds.min;
        var max = _SpriteRenderer.bounds.max;
        Vector2 pos = new Vector2();
        pos.x = Random.Range(min.x, max.x);
        pos.y = Random.Range(min.y, max.y);
        return pos;
    }
}
