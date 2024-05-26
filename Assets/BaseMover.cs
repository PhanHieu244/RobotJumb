using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BaseMover : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Transform standing;

    public Vector3 StandingPos => standing.position;
    
    private void Awake()
    {
        scoreText.gameObject.SetActive(false);
    }

    public Tween ShowText(int score)
    {
        scoreText.SetText("+" + score);
        scoreText.gameObject.SetActive(true);
        scoreText.transform.localScale = Vector3.one * 0.01f;
        return DOTween.Sequence()
            .Append(scoreText.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad))
            .AppendInterval(0.15f)
            .Append(scoreText.DOFade(0f, 0.2f).SetEase(Ease.InQuad))
            .AppendCallback(() =>
            {
                scoreText.gameObject.SetActive(false);
            });
    }

    public void Move(float speed = 2f)
    {
        StartCoroutine(IEMove(transform, Vector2.right, speed));
    }

    public void Stop()
    {
        StopAllCoroutines();
    }
    
    private IEnumerator IEMove(Transform mover, Vector2 direction, float speed)
    {
        // Normalize the direction to ensure consistent speed
        direction.Normalize();
    
        // Loop indefinitely until the coroutine is stopped
        while (true)
        {
            // Calculate the movement vector based on direction and speed
            Vector2 movement = direction * speed * Time.deltaTime;
        
            // Update the mover's position
            mover.position = (Vector2)mover.position + movement;
        
            // Yield until the next frame
            yield return null;
        }
    }
    
}
