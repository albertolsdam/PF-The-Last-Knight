using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    public enum Direccion
    {
        ARRIBA,
        IZQUIERDA,
        ABAJO,
        DERECHA
    }

    public Direccion direccionPuerta;

    public RoomTemplates templates;
    private bool spawned;

    private void Start()
    {
        Destroy(gameObject, 4f);
        templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        spawned = false;
        Invoke("Spawn", 0.2f);
    }

    private void Spawn()
    {
        if (!spawned)
        {
            switch (direccionPuerta)
            {
                case Direccion.ABAJO:
                    Instantiate(templates.bottomRooms[Random.Range(0, templates.bottomRooms.Count)], transform.position, Quaternion.identity);
                    break;
                case Direccion.ARRIBA:
                    Instantiate(templates.topRooms[Random.Range(0, templates.topRooms.Count)], transform.position, Quaternion.identity);
                    break;
                case Direccion.IZQUIERDA:
                    Instantiate(templates.leftRooms[Random.Range(0, templates.leftRooms.Count)], transform.position, Quaternion.identity);
                    break;
                case Direccion.DERECHA:
                    Instantiate(templates.rightRooms[Random.Range(0, templates.rightRooms.Count)], transform.position, Quaternion.identity);
                    break;
            }

            spawned = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (!other.GetComponent<SpawnRoom>().spawned)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            spawned = true;
        }
    }
}
