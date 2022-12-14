using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.UI;

public class FetchTokenData : MonoBehaviour
{
    /// <summary>
    /// This is an API example for fetching token data on PancakeSwap :) 
    /// </summary>


    // This is the token contract we want to look up.

    public string TokenAddress;


    // Our UI Elements to display the information for the referenced token

    public TextMeshProUGUI tokenName;
    public TextMeshProUGUI tokenSymbol;
    public TextMeshProUGUI tokenPriceUSD;
    public TextMeshProUGUI tokenPriceBNB;


    // Out input field to determine the token we're fetching data for

    public TMP_InputField GetAddress;


    // This is the Raw Response for our API call

    [Serializable]
    public class TokenData
    {
        public Data data;
    }

    // This is the "data" array that holds our token information. 

    [Serializable]
    public class Data
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public string price { get; set; }
        public string price_BNB { get; set; }
    }


    public void Start()
    {
        // Clear our token variable

        TokenAddress = "";


        // Clear our UI text

        tokenName.text = "";
        tokenSymbol.text = "";
        tokenPriceUSD.text = "";
        tokenPriceBNB.text = "";
    }


    // We run this function when the player clicks the "Get Data" button
    public void UpdateAndFetch()
    {
        TokenAddress = GetAddress.text;

        StartCoroutine(RequestData("https://api.pancakeswap.info/api/v2/tokens/" + TokenAddress));

        Debug.Log(TokenAddress);
    }

    // Call the API and fetch the data
    IEnumerator RequestData(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success: // If our API call is a success...

                    // Deserialize the JSON Data.

                    TokenData _tokenData = JsonConvert.DeserializeObject<TokenData>(webRequest.downloadHandler.text);


                    // Change our UI elements to show the token information

                    tokenName.text = _tokenData.data.name;
                    tokenSymbol.text = _tokenData.data.symbol;
                    tokenPriceUSD.text = _tokenData.data.price + " USD";
                    tokenPriceBNB.text = _tokenData.data.price_BNB + " BNB";


                    // Debug for sanity

                    Debug.Log(_tokenData.data.name);
                    Debug.Log(_tokenData.data.symbol);
                    Debug.Log(_tokenData.data.price + " USD");
                    Debug.Log(_tokenData.data.price_BNB + " BNB");

                    break;
            }
        }
    }
}
