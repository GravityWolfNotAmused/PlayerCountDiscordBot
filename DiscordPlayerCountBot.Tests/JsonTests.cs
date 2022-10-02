using PlayerCountBot.Json;

namespace PlayerCountBot.Tests;

[Collection("Json Serialization Test Suite")]
public class JsonTests
{
    [Fact(DisplayName = "Test Primitives Float", Timeout = 30)]
    public void SerializePrimitivesFloat()
    {
        var testFloat = JsonHelper.DeserializeObject<float>("2.23");

        Assert.True(testFloat == (float)2.23, $"{testFloat}");
        Assert.True(testFloat.GetType() == typeof(float));
    }

    [Fact(DisplayName = "Test Primitives Double", Timeout = 30)]
    public void SerializePrimitivesDouble()
    {
        var testDouble = JsonHelper.DeserializeObject<double>("200.00");

        Assert.True(testDouble == 200.00);
        Assert.True(testDouble.GetType() == typeof(double));

    }

    [Fact(DisplayName = "Test Primitives Char", Timeout = 30)]
    public void SerializePrimitivesChar()
    {
        var testChar = JsonHelper.DeserializeObject<char>("3");

        Assert.True(testChar == '3');
        Assert.True(testChar.GetType() == typeof(char));
    }

    [Fact(DisplayName = "Test Primitives Bool", Timeout = 30)]
    public void SerializePrimitivesBool()
    {
        var testBooleanTrue = JsonHelper.DeserializeObject<bool>("true");

        Assert.True(testBooleanTrue);
        Assert.True(testBooleanTrue.GetType() == typeof(bool));

        var testBooleanFalse = JsonHelper.DeserializeObject<bool>("false");

        Assert.True(!testBooleanFalse);
        Assert.True(testBooleanFalse.GetType() == typeof(bool));
    }

    [Fact(DisplayName = "Test Primitives Int", Timeout = 30)]
    public void SerializePrimitivesInt()
    {
        var testInt = JsonHelper.DeserializeObject<int>("100");

        Assert.True(testInt == 100);
        Assert.True(testInt.GetType() == typeof(int));
    }

    [Fact(DisplayName = "Test Strings", Timeout = 30)]
    public void SerializeStrings()
    {
        var testString = JsonHelper.DeserializeObject<string>("test") ?? "";

        Assert.True(!string.IsNullOrEmpty(testString), "Should not be null or empty.");
        Assert.True(!string.IsNullOrWhiteSpace(testString), "Should not be null or whitespace.");
        Assert.Equal("test", testString, true, true, true);
        Assert.True(testString.GetType() == typeof(string));
    }
}