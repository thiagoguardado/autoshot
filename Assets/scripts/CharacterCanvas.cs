using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCanvas : MonoBehaviour {

    public CharacterCanvasView CanvasPrefab;
    public Transform CanvasAnchor;

	// Use this for initialization
	void Awake() {

        CharacterCanvasView ccv = Instantiate(CanvasPrefab, CanvasAnchor.position, CanvasPrefab.transform.rotation, CanvasAnchor);
        ccv.Init();

    }

}
