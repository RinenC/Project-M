using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Realtime;
using Photon.Pun;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using ExitGames.Client.Photon;
using TMPro;

public class ChatManager : MonoSingleton<ChatManager>, IChatClientListener
{
    private ChatClient chatClient;
    private string userName;
    private string currentChannelName;

    public TMP_InputField inputFieldChat;
    public TMP_Text currentChannelText;
    public TMP_Text outputText;

    public bool isConnect = false;
    public bool isSend = false;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;

        // 유저 닉네임은 서버에 접속하면 변경하기 힘들다
        // 테스트시 유저의 구분을 위하여 현재 시간을 유저명으로 임시 설정
        userName = GameManager.Instance.playerName;
        currentChannelName = "Channel 001";

        chatClient = new ChatClient(this);

        // true 가 아닌 경우 어플이 백그라운드로 갈 때 연결이 끊어진다
        chatClient.UseBackgroundWorkerForSending = true;
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(userName));
        AddLine(string.Format("연결시도", userName));
    }

    // 현재 채팅 상태를 출력해줄 UI.Text
    public void AddLine(string lineString)
    {
        outputText.text += lineString + "\r\n";
    }

    // 어플리케이션이 종료되었을 때 호출
    public void OnApplicationQuit()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    // DebugLevel 에 정의 된 enum 타입에 따라 메세지를 출력한다
    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    // 서버에 연결을 성공함
    public void OnConnected()
    {
        AddLine("서버에 연결되었습니다.");

        // 지정한 채널명으로 접속
        chatClient.Subscribe(new string[] { currentChannelName }, 10);
        isConnect = true;
    }

    // 서버와의 연결이 끊어짐
    public void OnDisconnected()
    {
        AddLine("서버에 연결이 끊어졌습니다.");
        isConnect = false;
    }

    // 현재 클라이언트의 상태를 출력
    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("채널 입장 ({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("채널 퇴장 ({0})", string.Join(",", channels)));
    }

    // Update() 의 chatClient.Service() 가
    // 매 호출 시 OnGetMessages 를 호출한다.
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(currentChannelName))
        {
            // update text
            this.ShowChannel(currentChannelName);
        }
    }
    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        this.currentChannelName = channelName;
        // 채널에 저장 된 모든 채팅 메세지를 불러온다.
        // 유저 이름과 채팅 내용이 한꺼번에 불러와진다.
        this.currentChannelText.text = channel.ToStringMessages();
        Debug.Log("ShowChannel: " + currentChannelName);
    }


    // 개인 메세지를 보낼 경우 사용하는 메소드
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    // 포톤 엔진 공식 홈페이지에도 기술되어 있는 내용이다
    // chatClient.Service() 를 Update 에서 호출하던지
    // 필요에 따라 chatClient.Service() 를 반드시 호출 해야한다
    void Update()
    {
        chatClient.Service();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }


    // 인스펙터의 InputField 에서 입력받은 메세지를 보낼 때 사용한다.
    // InputField 의 On End Edit 에 이 메소드를 할당하자.
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            this.SendChatMessage(this.inputFieldChat.text);
            isSend = true;
            this.inputFieldChat.text = "";
        }
    }

    // 입력한 채팅을 서버로 전송한다.
    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }

        this.chatClient.PublishMessage(currentChannelName, inputLine);
    }
}