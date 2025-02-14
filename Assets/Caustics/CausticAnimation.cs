using UnityEngine;

public class CausticsAnimator : MonoBehaviour
{
    public Material causticsMaterial;
    public float speedX = 0.002f;
    public float speedY = 0.0015f;

    private Vector2 offset = Vector2.zero;

    void Update()
    {
        if (causticsMaterial != null)
        {
            offset.x += Time.deltaTime * speedX;
            offset.y += Time.deltaTime * speedY;

            causticsMaterial.SetVector("_CausticsOffset", offset);
        }
    }
}
