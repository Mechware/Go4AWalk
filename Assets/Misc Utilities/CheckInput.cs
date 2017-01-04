using UnityEngine;
using System.Collections;
using System;

public class CheckInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private static bool checkedInput = false;
    private static Collider2D hitCollider = null;

    public static void checkTapOrMouseDown(Action<Collider2D> colliderHitMethod) {

        // Optimized so that hit only needs to be tested once
        if(checkedInput) {
            if (hitCollider != null)
                colliderHitMethod(hitCollider);
            return;
        }
        checkedInput = true;

#if DEBUG || UNITY_WEBGL
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
            if (hitInfo) {
                hitCollider = hitInfo.collider;
                colliderHitMethod(hitInfo.collider);
            }
        }
#else
        for (var i = 0 ; i < Input.touchCount ; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
                if (hitInfo) {
                    hitCollider = hitInfo.collider;
                    colliderHitMethod(hitInfo.collider);
                }
            }
        }
#endif
    }

    void LateUpdate() {
        checkedInput = false;
        hitCollider = null;
    }
}
