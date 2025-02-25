using Model;
using Persistance;
using Moq;


namespace MineSweeperTest
{
    [TestClass]
    public class UnitTest1
    {
        private MineModel _model = null!;
        private MineTable _mockedTable = null!;
        private Mock<IMineDataAccess> _mock = null!;
        private IMineFileDataAccess _dataAcc = new IMineFileDataAccess();

        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new MineTable(10);
            _mockedTable.SetValue(9, 1, 0);
            _mockedTable.SetValue(9, 1, 0);
            _mockedTable.SetValue(9, 2, 1);

            _mock = new Mock<IMineDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_mockedTable));

            _model = new MineModel(_mock.Object, 10);
        }

        [TestMethod]
        public void Table6Test()
        {
            _model = new MineModel(null, 6);

            int emptyFields = 0;

            for (int i = 0; i < _model.Table.GetSize; ++i)
            {
                for (int j = 0; j < _model.Table.GetSize; ++j)
                {
                    if (_model.Table[i, j] == 0)
                    {
                        emptyFields++;
                    }
                }
            }
            Assert.AreEqual(36, emptyFields);
        }

        [TestMethod]
        public void Table10Test()
        {
            _model = new MineModel(null, 10);

            int emptyFields = 0;

            for (int i = 0; i < _model.Table.GetSize; ++i)
            {
                for (int j = 0; j < _model.Table.GetSize; ++j)
                {
                    if (_model.Table[i, j] == 0)
                    {
                        emptyFields++;
                    }
                }
            }
            Assert.AreEqual(100, emptyFields);
        }

        [TestMethod]
        public void Table16Test()
        {
            _model = new MineModel(null, 16);

            int emptyFields = 0;

            for (int i = 0; i < _model.Table.GetSize; ++i)
            {
                for (int j = 0; j < _model.Table.GetSize; ++j)
                {
                    if (_model.Table[i, j] == 0)
                    {
                        emptyFields++;
                    }
                }
            }
            Assert.AreEqual(256, emptyFields);
        }

        [TestMethod]
        public void StepTest()
        {
            _model = new MineModel(_dataAcc, 10);
            _model.LoadGameAsync(@"..\..\..\..\test2.stl");

            int currentPlayer = _model.Table.GetPlayer;
            _model.Step(3, 0);
            _model.ShowAll(3, 0);

            Assert.AreEqual(2, _model.Table.GetValue(3, 0));
            Assert.IsTrue(_model.Table.IsOpened(3, 0));
            Assert.AreEqual(currentPlayer, _model.Table.GetPlayer == 1 ? 2 : 1);
        }

        [TestMethod]
        public void GameOverTest()
        {
            _model = new MineModel(_dataAcc, 10);
            _model.LoadGameAsync(@"..\..\..\..\test2.stl");

            int currentPlayer = _model.Table.GetPlayer;
            _model.Step(3, 1);

            Assert.AreEqual(-1, _model.Table.GetValue(3, 1));
            Assert.IsTrue(_model.IsGameOver);
            Assert.AreEqual(currentPlayer, _model.Table.GetPlayer == 1 ? 2 : 1);
        }

        [TestMethod]
        public async Task Loadtest()
        {
            await _model.LoadGameAsync(String.Empty);

            for (int i = 0; i < _model.Table.GetSize - 1; ++i)
            {
                for (int j = 0; j < _model.Table.GetSize; ++j)
                {
                    Assert.AreEqual(_mockedTable.GetValue(i, j), _model.Table.GetValue(i, j));
                }
            }

            _mock.Verify(dataAccess => dataAccess.LoadAsync(String.Empty), Times.Once());
        }
    }
}