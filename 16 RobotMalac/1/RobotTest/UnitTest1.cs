using RobotmalacModel.Model;
using RobotmalacModel.Persistence;
using Moq;
using static RobotmalacModel.Persistence.RobotTable;
using System.Reflection;

namespace RobotTest
{
    [TestClass]
    public class UnitTest1
    {
        private RobotModel _model = null!;
        private RobotTable _mockedTable = null!;
        //private RobotTable _table = null!;
        private Mock<IRobotDataAccess> _mock = null!;
        private IRobotFileDataAccess _dataAcc = new IRobotFileDataAccess();

        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new RobotTable(4);

            _mock = new Mock<IRobotDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_mockedTable));

            _model = new RobotModel(_mock.Object, 4);
        }

        [TestMethod]
        public void Table1Test()
        {
            _model = new RobotModel(null, 4);
            _mockedTable= new RobotTable(4);
            
            Assert.AreEqual(_model.GetSize, _mockedTable.GetSize);
        }

        [TestMethod]
        public void Table2Test()
        {
            _model = new RobotModel(null, 6);
            _mockedTable= new RobotTable(6);

            Assert.AreEqual(_model.GetSize, _mockedTable.GetSize);
        }

        [TestMethod]
        public void Table3Test()
        {
            _model = new RobotModel(null, 8);
            _mockedTable= new RobotTable(8);

            Assert.AreEqual(_model.GetSize, _mockedTable.GetSize);
        }

        [TestMethod]
        public void TestStep()
        {
            RobotModel _model = new RobotModel(null, 6);

            _model.Step();

            // Ellenõrzi, hogy a játéktábla játékost váltott-e.
            Assert.AreEqual(2, _model.Table.GetPlayer);
        }

        [TestMethod]
        public void TestCollectCommand()
        {
            RobotModel model = new RobotModel(null, 8);

            model.CollectCommand("move left", 1);

            CollectionAssert.AreEqual(new string[] { "move left" }, model.P1StandaloneCMD);
        }

        [TestMethod]
        public void TestCollectCommand2()
        {
            RobotModel model = new RobotModel(null, 4);

            model.CollectCommand("move left,move down", 1);

            CollectionAssert.AreEqual(new string[] { "move left", "move down" }, model.P1StandaloneCMD);
        }

        [TestMethod]
        public void TestDoCommand()
        {
            RobotModel model = new RobotModel(null, 6);

            model.CollectCommand("move left,move down", 1);
            model.CollectCommand("move right,move up", 2);

            model.DoCommand();

            CollectionAssert.AreEqual(new int[] { 1, 1 }, model.Table.P1Current);
        }

        [TestMethod]
        public void TestDoCommand2()
        {
            RobotModel model = new RobotModel(null, 4);

            model.CollectCommand("move down,move down", 1);
            model.CollectCommand("move right,move up", 2);

            model.DoCommand();

            CollectionAssert.AreEqual(new int[] { 1, 2 }, model.Table.P1Current);
            CollectionAssert.AreEqual(new int[] { 3, 2 }, model.Table.P2Current);
        }

        [TestMethod]
        public void Win()
        {
            RobotModel model = new RobotModel(null, 4);
            int currentPlayer = model.Table.GetPlayer;

            model.CollectCommand("move right", 1);
            model.CollectCommand("shoot,shoot,shoot", 2);

            model.DoCommand();

            Assert.IsTrue(model.IsGameOver);
            Assert.AreEqual(currentPlayer, model.Table.GetPlayer == 1 ? 1 : 2);
        }

        [TestMethod]
        public void Win2()
        {
            RobotModel model = new RobotModel(null, 4);

            model.CollectCommand("move right", 1);
            model.CollectCommand("shoot,shoot,shoot", 2);

            model.DoCommand();

            Assert.IsTrue(model.IsGameOver);
            Assert.AreEqual(model.Table.P1Health, 0);
        }

        [TestMethod]
        public async Task StepTest() 
        {
            _model = new RobotModel(_dataAcc, 4);
            await _model.LoadGameAsync(@"..\..\..\..\test2");

            int currentPlayer = _model.Table.GetPlayer;
            _model.CollectCommand("move right", 1);
            _model.CollectCommand("punch,punch,punch", 2);

            _model.DoCommand();

            Assert.AreEqual(_model.Table.P1Health, 0);
            Assert.IsTrue(_model.IsGameOver);
            Assert.AreEqual(currentPlayer, _model.Table.GetPlayer == 1 ? 1 : 2);
        }


    }
}