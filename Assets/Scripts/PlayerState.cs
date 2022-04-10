using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[System.Serializable]
public struct PlayerInfo
{
    public string name;
    public float points;
    public List<Tile> tilesTaken;

}

public class PlayerState : MonoBehaviour
{
    public Player playerClass;
    public PlayerCamera cameraClass;

    public string name;
    
    private Player player;
    private PlayerCamera camera;

    [HideInInspector]
    public float points = 0;
    public UnityAction<float> OnPointGot;

    public void Awake()
    {
        Assert.IsNotNull(playerClass);
        Assert.IsNotNull(cameraClass);

        //player = CreatePlayer();
        //camera = CreateCamera(player.gameObject);
        
    }

    private Player CreatePlayer()
    {
        Player Player = Instantiate(playerClass);
        Player.Owner = this;

        return Player;
    }
    private PlayerCamera CreateCamera(GameObject Target)
    {
        PlayerCamera Camera = Instantiate(cameraClass, player.gameObject.transform);
        Camera.Target = Target;

        return Camera;
    }

    private void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo))
        {

            GameObject tileTouched = hitInfo.collider.transform.gameObject;

            if (Input.GetMouseButtonDown(0))
            {
                MeshRenderer mr = tileTouched.GetComponentInChildren<MeshRenderer>();
                mr.material.color = Color.red;

                Debug.Log(tileTouched.name);
            }
        }
    }
}
