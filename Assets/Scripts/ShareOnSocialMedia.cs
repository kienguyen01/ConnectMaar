using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShareOnSocialMedia : MonoBehaviour
{
    // Sending the inv link
    [SerializeField] GameObject sahreBtn;
    [SerializeField] Text PlayerName;
    [SerializeField] Text InvLink;

    // Sharing the score
    [SerializeField] Text MovesTaken;
    [SerializeField] Text ObjectivesTaken;
    [SerializeField] Text Opponent;



    public void SendInvLink()
    {
        //new NativeShare().SetSubject("Game Invite").SetText(PlayerName + " would like to invite you to a game").SetUrl()
    }




    public void SendWinnerScreen()
    {
        StartCoroutine(ScreenshotAndShare());
    }


    //TAKE SCREENSHOT AND THEN SHARE THE GAME
    private IEnumerator ScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D SS = new Texture2D( Screen.width, Screen.height, TextureFormat.ARGB32, false );
        SS.ReadPixels  (new Rect(0,0,Screen.width,Screen.height), 0,0);
        SS.Apply();

        string filepath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filepath, SS.EncodeToPNG());

        Destroy(SS);

        new NativeShare().AddFile(filepath).SetSubject("Winner").SetText("Winner Winner Chicken Dinner").Share();
    }






}
