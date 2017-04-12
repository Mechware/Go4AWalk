using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

    private AnimatorOverrideController controller;
    public AnimationClip[] replacementClips;

	// Use this for initialization
	void Start () {
        Animator animator = GetComponentInChildren<Animator>();
        controller = new AnimatorOverrideController();
        controller.runtimeAnimatorController = animator.runtimeAnimatorController;
        animator.runtimeAnimatorController = controller;
        setController();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void setController() {

        // get the number of clips on the AnimatorOverrideController
        int numClips = controller.clips.Length;

        // make a new array of that length
        AnimationClipPair[] newClips = new AnimationClipPair[numClips];

        // loop through each clip pair
        for (int i = 0 ; i < numClips ; i++) {
            AnimationClipPair pair = controller.clips[i];

            // get the appropriate clip from your model
            string name = pair.originalClip.name;
            AnimationClip clip = getClipWithName(name);
            if(clip == null) {
                print("ERR: Couldn't find clip with name: " + name);
                continue;
            }

            // if there is a model clip, 
            // set it as the overrideClip on the pair
            if (clip != null) {
                pair.overrideClip = clip;
            }

            // save the pair to your new clips array
            newClips[i] = pair;

        }

        // when everything is properly set, 
        // replace the entire clips array          
        controller.clips = newClips;
    }

    AnimationClip getClipWithName(string name) {
        foreach(AnimationClip clip in replacementClips) {
            if (clip == null) continue;
            if (name.Equals(clip.name))
                return clip;
        }
        return null;
    }
}
