using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;

public class PlayerListItem : MonoBehaviour
{

    public string PlayerName;
    public int ConnectionId;
    public int PlayerId;
    public ulong PlayerSteamId;
    public bool AvatarReceived;

    public TextMeshProUGUI PlayerNameText;
    public RawImage PlayerIcon;
    public TextMeshProUGUI PlayerStatusText;
    public bool PlayerStatus;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    public void ChangeReadyStatus()
    {
        if (PlayerId == 1){
            PlayerStatusText.text = "";
        }
        else if (PlayerStatus)
        {
            PlayerStatusText.text = "Ready";
        }
        else
        {
            PlayerStatusText.text = "Not Ready";
        }
    }

    void Start()
    {

        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);

    }

    public void SetPlayerValues()
    {

        PlayerNameText.text = PlayerName;
        ChangeReadyStatus();
        if (!AvatarReceived) { GetPlayerIcon(); }

    }

    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {

        if (callback.m_steamID.m_SteamID == PlayerSteamId)
        {
            PlayerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
        }
        else
        {
            return;
        }

    }

    void GetPlayerIcon()
    {

        int ImageId = SteamFriends.GetLargeFriendAvatar((CSteamID) PlayerSteamId);
        if (ImageId == -1){ return; }
        PlayerIcon.texture = GetSteamImageAsTexture(ImageId);

    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        AvatarReceived = true;
        return texture;
    }
}
