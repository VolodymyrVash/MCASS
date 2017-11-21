using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Gurock.TestRail;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class TestRailTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    var caseID = "46760";

	    ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
	    APIClient client = new APIClient("https://qa.room8studio.com/");
	    client.User = "e.kalina@room8studio.com";
	    client.Password = "TOpsexIds.wd1TuNECt7-v.Mm.hsuW8kJTCuAV2iX";

	    var data = new Dictionary<string, object>
	    {
	        { "name", "Automation Test Run 20.11" },
	        { "suite_id", 369 },
	    };

	    try
	    {
	        JContainer jContainer = (JContainer)client.SendPost("add_run/2", data);

	    }
	    catch (Exception e)
	    {
	        Debug.LogError(e);
	    }

	    var response = client.SendGet("get_runs/2&is_completed=0").ToString();

	    var array = JsonConvert.DeserializeObject<List<TestSuite.TestRun>>(response);

	    var InArray = array[0];
	    var testRunName = InArray.name;

	    if (testRunName == "Automation Test Run 20.11")
	    {
	        TestSuite.RequestsToTestRail.TestPass();
	    }
	    else
	    {
	        TestSuite.RequestsToTestRail.TestFail();
	    }
    }

    public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }
}
