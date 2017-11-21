using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
/*
public class UITestExample : UITest
{
    [UISetUp]
    public IEnumerable Init()
    {

#if UNITY_EDITOR
        yield return LoadSceneByPath("Assets/Game/Scene/ModeSelector.unity");
#else
        yield return LoadScene("ModeSelector");
#endif
    }
    
    [UITest]
    public IEnumerable SecondScreenCanBeOpenedFromTheFirstOne()
    {
        // Wait until object with given component appears in the scene
        yield return WaitFor(new ObjectAppeared<FirstScreen>());

        // Wait until button with given name appears and simulate press event
        yield return Press("Button-OpenSecondScreen");

        yield return WaitFor(new ObjectAppeared<SecondScreen>());

        // Wait until Text component with given name appears and assert its value
        yield return AssertLabel("SecondScreen/Text", "Second screen");

        yield return Press("Button-Close");

        // Wait until object with given component disappears from the scene
        yield return WaitFor(new ObjectDisappeared<SecondScreen>());
    }

    [UITest]
    public IEnumerable SuccessfulNetworkResponseIsDisplayedOnTheFirstScreen()
    {
        yield return WaitFor(new ObjectAppeared<FirstScreen>());

        // Predefine the mocked server response
        mockNetworkClient.mockResponse = "Success!";

        yield return Press("Button-NetworkRequest");

        // Check the response displayed on UI
        yield return AssertLabel("FirstScreen/Text-Response", "Success!");

        // Assert the requested server parameter
        Assert.AreEqual(mockNetworkClient.mockRequest, "i_need_data");
    }

    [UITest]
    public IEnumerable FailingBoolCondition()
    {
        yield return WaitFor(new ObjectAppeared("FirstScreen"));
        var s = FindObjectOfType<FirstScreen>();

        // Wait until FirstScene component is disabled, this line will fail by timeout
        // BoolCondition can be used to wait until any condition is satisfied
        yield return WaitFor(new BoolCondition(() => !s.enabled));
    }
    
    
    [UITest]
    public IEnumerable WaitForGameEnd()
    {
        var pause = new WaitForSeconds(5.5f);
        yield return WaitFor(new ObjectAppeared("Canvas"));

        yield return Press("AdditionButton");
        yield return pause;

        yield return AssertLabel("ScoreBackImage/ScoreText", "SCORE");

    }

    [UITest]
    public IEnumerable GameStart()
    {
        var pause = new WaitForSeconds(4.0f);
        yield return WaitFor(new ObjectAppeared("Canvas"));

        yield return Press("AdditionButton");
        yield return pause;

        yield return AssertLabel("ScoreImage/ScoreText", "0");

    }

    [UITest]
    public IEnumerable GameWithPlus()
    {
        var pause = new WaitForSeconds(0.5f);
        yield return WaitFor(new ObjectAppeared("Canvas"));

        yield return Press("AdditionButton");
        yield return pause;

        yield return AssertLabel("MathsSymbol/Pluss", "+");

    }
    

   [UITest]
    public IEnumerable GameWithMinus()
    {
        var pause = new WaitForSeconds(0.5f);
        yield return WaitFor(new ObjectAppeared("Canvas"));

        yield return Press("SubtractionButton");
        yield return pause;

        yield return AssertLabel("MathsSymbol/Minuss", "-");

    }
    
    [UITest]
    public IEnumerable GameWithMultiplication()
    {
        var pause = new WaitForSeconds(1.5f);
        yield return WaitFor(new ObjectAppeared("Canvas"));

        yield return Press("MultiplicationButton");
        yield return pause;

        yield return AssertLabel("MathsSymbol/Multiplication", "*");

    }

    [UITest]
    public IEnumerable GameWithDivision()
    {
        var pause = new WaitForSeconds(1.5f);
        yield return WaitFor(new ObjectAppeared("Canvas"));

        yield return Press("MultiplicationButton");
        yield return pause;

        yield return AssertLabel("MathsSymbol/Division", "/");

    }

    [UITest]
    public IEnumerable MixButton()
    {
       
        var pause = new WaitForSeconds(1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return pause;
        yield return AssertLabel("Canvas/Mixes", "Miks");

    }

    [UITest]
    public IEnumerable END()
    {
        var pause = new WaitForSeconds(1.5f);
        yield return WaitFor(new ObjectAppeared("Canvas"));

        yield return pause;

    }

    

}
*/
