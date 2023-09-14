using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingEffects : MonoBehaviour
{

    public Volume volume;

    void Awake(){
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out Bloom bloom);
        bloom.intensity.value = 2.0f;
        bloom.scatter.value = 1.0f; // 0.0f to 1.0f
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
