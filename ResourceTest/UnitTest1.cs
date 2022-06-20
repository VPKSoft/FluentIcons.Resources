namespace ResourceTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        try
        {
            _ = FluentIcons.Resources.Filled.Size16.ic_fluent_play_16_filled;
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }
}