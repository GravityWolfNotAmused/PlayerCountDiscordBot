namespace PlayerCountBot.Tests
{
    [Collection("Sun & Moon Tag Test Suite")]
    public class SunMoonTagTests
    {
        [Theory(DisplayName = "Test If Should Be Moon", Timeout = 30)]
        [InlineData("05:30")]
        [InlineData("21:30")]
        [InlineData("23:30")]
        public void ShouldBeMoonPhase(string time)
        {
            var success = time.TryGetSunMoonPhase(null, null, out var output);

            Assert.True(success, "SunMoon Phase failed to parse");
            Assert.True(!string.IsNullOrEmpty(output));
            Assert.Equal("🌙", output);
        }

        [Theory(DisplayName = "Test If Should Be Sun", Timeout = 30)]
        [InlineData("06:04")]
        [InlineData("15:42")]
        [InlineData("19:06")]
        public void ShouldBeSunPhase(string time)
        {
            var success = time.TryGetSunMoonPhase(null, null, out var output);

            Assert.True(success, "SunMoon Phase failed to parse");
            Assert.True(!string.IsNullOrEmpty(output));
            Assert.Equal("☀️", output);
        }

        [Theory(DisplayName = "Test Invalid Values", Timeout = 30)]
        [InlineData("abc")]
        [InlineData("")]
        [InlineData("not a time value")]
        [InlineData("24:30")]
        public void ShouldNotParse(string time)
        {
            var success = time.TryGetSunMoonPhase(null, null, out var output);

            Assert.False(success, "SunMoon Phase successfully parsed when it should have failed");
            Assert.True(string.IsNullOrEmpty(output), "SunMoon output is not null or empty.");
        }

        [Theory(DisplayName = "Test custom sunrise and sunset", Timeout = 30)]
        [InlineData("05:30")]
        [InlineData("21:30")]
        [InlineData("23:30")]
        [InlineData("06:04")]
        [InlineData("15:42")]
        [InlineData("19:06")]
        public void ShouldOutputCorrectValue(string time)
        {
            var information = new BotInformation()
            {
                SunriseHour = 1,
                SunsetHour = 21
            };

            var success = time.TryGetSunMoonPhase(information.SunriseHour, information.SunsetHour, out var output);
            var manualProcessingSuccess = TimeOnly.TryParse(time, out var timeOutput);

            Assert.True(success, "SunMoon Phase parsing failed");
            Assert.True(manualProcessingSuccess, "Manual Parsing SunMoon Phase failed");

            var isBetween = timeOutput.Hour >= information.SunriseHour && timeOutput.Hour < information.SunsetHour;
            var sunMoonPhase = isBetween ? "☀️" : "🌙";

            if (isBetween)
                Assert.True(sunMoonPhase.Equals("☀️"), "Time was between sunrise and sunset, but was not a Sun.");

            if (!isBetween)
                Assert.True(sunMoonPhase.Equals("🌙"), "Time not was between sunrise and sunset, but was not a Moon.");

            Assert.Equal(sunMoonPhase, output);
        }
    }
}