using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimarLogoVictoria : MonoBehaviour
{
    public IEnumerator IniciarAnimacion(Vector3 posicionFinal)
    {
        while (MoverHaciaPosicion(posicionFinal, 100))
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);
    }

    private bool MoverHaciaPosicion(Vector3 objetivo, float velocidad)
    {
        transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);
        return (objetivo != transform.position);
    }
}
