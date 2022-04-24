using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private Text damageAmount;
    private float scaleFactor;

    private void OnEnable()
    {
        StartCoroutine(DisableGameObject(1));
        scaleFactor = 1;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    private void Update()
    {
        scaleFactor -= Time.deltaTime;
        if (scaleFactor < 0) scaleFactor = 0;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    public void SetDamageAmount(float _damageAmount)
    {
        damageAmount.text = _damageAmount.ToString();
    }

    public void SetColor(Color _color)
    {
        damageAmount.color = _color;
    }

    private IEnumerator DisableGameObject(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        damageAmount.text = null;
        gameObject.SetActive(false);
    }
}
