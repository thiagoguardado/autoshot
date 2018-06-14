using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCanvas : MonoBehaviour {

    public WeaponCanvas CanvasPrefab;
    public Transform CanvasAnchor;

	// Use this for initialization
	void Awake() {

        WeaponCanvas wc = Instantiate(CanvasPrefab, CanvasAnchor.position, CanvasPrefab.transform.rotation, CanvasAnchor);
        wc.Init();

        EnergyCanvas ec = wc.GetComponent<EnergyCanvas>();
        if (ec != null)
            ec.Init();

    }

}
