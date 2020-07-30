using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum NodeStates
{
    Locked, 
    Visited,
    Attainable
}

public class MapNode : MonoBehaviour
{
    public SpriteRenderer sr;
    public SpriteRenderer visitedCircle;
    public Image visitedCircleImage;

    public Node Node { get; private set; }
    public NodeBlueprint Blueprint { get; private set; }

    private float initialScale;
    private const float HoverScaleFactor = 1.2f;
    private float mouseDownTime;

    private const float MaxClickDuration = 0.5f;

    //Mi codigo
    public string scene;
    public Escenas escenas;
    public NodeStates estado;
    public bool cargarEscena = false;
    public MapManager mapManager;

    public void SetUp(Node node, NodeBlueprint blueprint)
    {
        mapManager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>();

        Node = node;
        Blueprint = blueprint;
        sr.sprite = blueprint.sprite;
        if (node.nodeType == NodeType.Boss) transform.localScale *= 1.5f;
        initialScale = sr.transform.localScale.x;
        visitedCircle.color = MapView.Instance.visitedColor;
        visitedCircle.gameObject.SetActive(false);
        SetState(NodeStates.Locked);

        escenas = GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>();

        switch(blueprint.nodeType)
        {
            case NodeType.MinorEnemy:
                scene = escenas.escenarioActual.escenasEnemigos[UnityEngine.Random.Range(0, escenas.escenarioActual.escenasEnemigos.Count)];
                break;

            case NodeType.EliteEnemy:
                scene = escenas.escenarioActual.escenasEnemigosElites[UnityEngine.Random.Range(0, escenas.escenarioActual.escenasEnemigosElites.Count)];
                break;

            case NodeType.Boss:
                scene = escenas.escenarioActual.escenasBosses[UnityEngine.Random.Range(0, escenas.escenarioActual.escenasBosses.Count)];
                break;
               
            case NodeType.Treasure:
                scene = escenas.escenarioActual.escenasTesoros[UnityEngine.Random.Range(0, escenas.escenarioActual.escenasTesoros.Count)];
                break;

            case NodeType.Store: 
                break;

            case NodeType.RestSite:
                scene = escenas.escenarioActual.escenaHoguera;
                break;

            case NodeType.Mystery: 
                break;
        }
    }

    public void SetState(NodeStates state)
    {
        visitedCircle.gameObject.SetActive(false);
        switch (state)
        {
            case NodeStates.Locked:
                estado = NodeStates.Locked;

                sr.DOKill();
                sr.color = MapView.Instance.lockedColor;
                break;
            case NodeStates.Visited:
                estado = NodeStates.Visited;

                sr.DOKill();
                sr.color = MapView.Instance.visitedColor;
                visitedCircle.gameObject.SetActive(true);
                break;
            case NodeStates.Attainable:
                estado = NodeStates.Attainable;

                // start pulsating from visited to locked color:
                sr.color = MapView.Instance.lockedColor;
                sr.DOKill();
                sr.DOColor(MapView.Instance.visitedColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnMouseEnter()
    {
        sr.transform.DOKill();
        sr.transform.DOScale(initialScale * HoverScaleFactor, 0.3f);
    }
    
    private void OnMouseExit()
    {
        sr.transform.DOKill();
        sr.transform.DOScale(initialScale, 0.3f);
    }

    private void OnMouseDown()
    {
        mouseDownTime = Time.time;
    }

    private void OnMouseUp()
    {
        if (Time.time - mouseDownTime < MaxClickDuration)
        {
            if (estado.Equals(NodeStates.Attainable))
            {
                cargarEscena = true;

                GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().clip = GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().clipCirculo;
                GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().Play();
                mapManager.SaveMap("MapaAntEst");
            }

            // user clicked on this node:
            MapPlayerTracker.Instance.SelectNode(this);

            if (cargarEscena)
            {
                cargarEscena = false;
                StartCoroutine(escenas.CargarEscena(scene));
            }
        }
    }

    public void ShowSwirlAnimation()
    {
        if (visitedCircleImage == null)
            return;
        
        const float fillDuration = 0.3f;
        visitedCircleImage.fillAmount = 0;

        DOTween.To(() => visitedCircleImage.fillAmount, x => visitedCircleImage.fillAmount = x, 1f, fillDuration);
    }
}
