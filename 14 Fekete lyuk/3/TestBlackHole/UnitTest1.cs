using BlackHoleModel.Model;
using BlackHoleModel.Persistence;
using Moq;
using System.Reflection;

namespace TestBlackHole
{
    [TestClass]
    public class UnitTest1
    {
        private GameModel _model = null!;
        private Table _mockedTable = null!;
        private Mock<IBlackHoleDataAccess> _mock = null!;
        private BlackHoleFileDataAccess _dataAcc = new BlackHoleFileDataAccess();

        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new Table(5);

            _mock = new Mock<IBlackHoleDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_mockedTable));

            _model = new GameModel(_mock.Object, 10);
        }

        [TestMethod]
        public void Table1Test()
        {
            _model = new GameModel(null, 5);
            _mockedTable= new Table(5);

            Assert.AreEqual(_model.GetSize, _mockedTable.GetSize);
        }

        [TestMethod]
        public void Table2Test()
        {
            _model = new GameModel(null, 7);
            _mockedTable= new Table(7);

            Assert.AreEqual(_model.GetSize, _mockedTable.GetSize);
        }

        [TestMethod]
        public void Table3Test()
        {
            _model = new GameModel(null, 9);
            _mockedTable= new Table(9);

            Assert.AreEqual(_model.GetSize, _mockedTable.GetSize);
        }

        [TestMethod]
        public void TestStep1()
        {
            _mockedTable= new Table(5);
            GameModel _model = new GameModel(null, 5);

            _model.GenerateFields();

            _model.Step(0, 0);
            _model.Step(0, 1);

            // Ellenõrzi, hogy a játéktábla játékost váltott-e.
            Assert.AreEqual(2, _model.Table.Player);
        }

        [TestMethod]
        public void TestStep2()
        {
            _mockedTable= new Table(9);
            GameModel _model = new GameModel(null, 9);

            _model.GenerateFields();

            _model.Step(0, 0);
            _model.Step(1, 0);

            // Ellenõrzi, hogy a játéktábla játékost váltott-e.
            Assert.AreEqual(2, _model.Table.GetTableValue(7, 0));
            Assert.AreEqual(0, _model.Table.GetTableValue(0, 0));
        }

        [TestMethod]
        public void TestStep3()
        {
            _mockedTable= new Table(7);
            GameModel _model = new GameModel(null, 7);

            _model.Step(0, 6);
            //CollectionAssert.AreEqual(new int[] { 0, 6 }, _model.Table.P1Current);
            _model.Step(0, 5);

            CollectionAssert.AreEqual(new int[] { -1, -1 }, _model.Table.P1Current);            
        }

        //[TestMethod]
        //public async Task LoadTest()
        //{
        //    _model = new GameModel(_dataAcc, 4);
        //    await _model.LoadGameAsync(@"..\..\..\..\..\..\1\t1-p1-win");

        //    int currentPlayer = _model.Table.Player;
        //    _model.Step(2, 1);
        //    Assert.AreEqual(currentPlayer, _model.Table.Player == 1 ? 1 : 2);
        //    _model.Step(2, 2);

        //    Assert.AreEqual(_model.Table.P1ShipsInHole, 2);
        //    Assert.IsTrue(_model.IsGameOver);
        //    Assert.AreEqual(2, _model.Table.Player == 1 ? 1 : 2);
        //}
    }
}