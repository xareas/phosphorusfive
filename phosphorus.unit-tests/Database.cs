
/*
 * phosphorus five, copyright 2014 - Mother Earth, Jannah, Gaia
 * phosphorus five is licensed as mit, see the enclosed LICENSE file for details
 */

using System;
using System.Reflection;
using NUnit.Framework;
using phosphorus.core;

namespace phosphorus.unittests
{
    /// <summary>
    /// [pf.data.xxx] unit tests
    /// </summary>
    [TestFixture]
    public class Database : TestBase
    {
        public Database ()
        {
            // making sure we load our necessary assemblies
            Loader.Instance.LoadAssembly ("phosphorus.types");
            Loader.Instance.LoadAssembly ("phosphorus.unit-tests");
            Loader.Instance.LoadAssembly ("phosphorus.hyperlisp");
            Loader.Instance.LoadAssembly ("phosphorus.lambda");
            Loader.Instance.LoadAssembly ("phosphorus.file");
            Loader.Instance.LoadAssembly ("phosphorus.data");
            _context = Loader.Instance.CreateApplicationContext ();
        }

        /*
         * runs before every unit test, deletes all documents from "unit_tests"
         */
        [SetUp]
        public void SetUp ()
        {
            // deleting entire database, in case there already are items in it
            ExecuteLambda (@"pf.data.delete:@/*/*/?node");
        }

        /*
         * necessary to return "root folder" of executing Assembly
         */
        [ActiveEvent (Name = "pf.core.application-folder")]
        private static void GetRootFolder (ApplicationContext context, ActiveEventArgs e)
        {
            string asmPath = Assembly.GetExecutingAssembly ().Location;
            asmPath = asmPath.Substring (0, asmPath.LastIndexOf ("/") + 1);
            e.Args.Value = asmPath;
        }

        /// <summary>
        /// selects non-existing objects from database, making sure nothing is returned
        /// </summary>
        [Test]
        public void Select01 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.select:@/*/*/_mumbo_field/=jumbo_value/?node");
            Assert.AreEqual (0, tmp [0].Count);
        }

        /// <summary>
        /// inserts node, then selects, for then to delete and select again, making
        /// sure both insert, select and delete works as it should
        /// </summary>
        [Test]
        public void Select02 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _test1
    howdy:world
pf.data.select:@/*/*/_test1/?node
pf.data.delete:@/*/*/_test1/?node
pf.data.select:@/*/*/_test1/?node");
            Assert.AreEqual (1, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test1", tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual (null, tmp [1] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy", tmp [1] [0] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world", tmp [1] [0] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual (0, tmp [3].Count, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts into database for then to select 'name', to verify select works as it should
        /// </summary>
        [Test]
        public void Select03 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _testX
    howdy:world
pf.data.select:@/*/*/_testX/0/?name");
            Assert.AreEqual (1, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy", tmp [1] [0].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts into database for then to select 'value', to verify select works as it should
        /// </summary>
        [Test]
        public void Select04 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _testX
    howdy:world
pf.data.select:@/*/*/_testX/0/?value");
            Assert.AreEqual (1, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world", tmp [1] [0].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts into database, for then to select 'count' to verify select works as it should
        /// </summary>
        [Test]
        public void Select05 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _testX
    howdy:world
pf.data.select:@/*/*/_testX/0/?count");
            Assert.AreEqual (1, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual (1, tmp [1] [0].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts into database, for then to select 'path' to verify select works as it should
        /// </summary>
        [Test]
        public void Select06 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _testX
    howdy:world
pf.data.select:@/*/*/_testX/0/?path");
            Assert.AreEqual (1, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.IsTrue (tmp [1] [0].Value is Node.DNA, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts multiple objects into database, for then to select 'name', having multiple return values, to
        /// verify select and insert works as it should
        /// </summary>
        [Test]
        public void Select07 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _testX
    howdy1:world1
  _testX
    howdy2:world2
pf.data.select:@/*/*/_testX/0/?name");
            Assert.AreEqual (2, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy1", tmp [1] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [1].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy2", tmp [1] [1].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts multiple objects into database, for then to select 'value', having multiple return values, to
        /// verify select and insert works as it should
        /// </summary>
        [Test]
        public void Select08 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _testX
    howdy1:world1
  _testX
    howdy2:world2
pf.data.select:@/*/*/_testX/0/?value");
            Assert.AreEqual (2, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world1", tmp [1] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [1].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world2", tmp [1] [1].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts multiple objects into database, for then to select 'path', having multiple return values, to
        /// verify select and insert works as it should
        /// </summary>
        [Test]
        public void Select09 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _testX
    howdy1:world1
  _testX
    howdy2:world2
pf.data.select:@/*/*/_testX/0/?path");
            Assert.AreEqual (2, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [1].Name, "wrong value of node after executing lambda object");
            Assert.IsTrue (tmp [1] [0].Value is Node.DNA, "wrong value of node after executing lambda object");
            Assert.IsTrue (tmp [1] [1].Value is Node.DNA, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts multiple objects into database, for then to select 'count', having multiple return values, to
        /// verify select and insert works as it should
        /// </summary>
        [Test]
        public void Select10 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _testX
    howdy1:world1
  _testX
    howdy2:world2
pf.data.select:@/*/*/_testX/?count");
            Assert.AreEqual (1, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual (string.Empty, tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual (2, tmp [1] [0].Value, "wrong value of node after executing lambda object");
        }
        
        /// <summary>
        /// inserts a couple of nodes, for then to select 'deep' from database, making
        /// sure select and insert works as it should
        /// </summary>
        [Test]
        public void Select11 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _test4
    howdy:world
  _test4
    howdy:world2
    query_field
      x:y
  _test4
    howdy:world3
pf.data.select:@/*/*/_test4/*/query_field/*/x/=y/././?node");
            Assert.AreEqual (1, tmp [1].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test4", tmp [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world2", tmp [1] [0] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual ("x", tmp [1] [0] [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("y", tmp [1] [0] [1] [0].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts from an expression source, making sure insert can handle expressions
        /// </summary>
        [Test]
        public void Insert01 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert:@/+/?node
_testX
  howdy:world
pf.data.select:@/*/*/_testX/?node");
            Assert.AreEqual (1, tmp [2].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_testX", tmp [2] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual (null, tmp [2] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy", tmp [2] [0] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world", tmp [2] [0] [0].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts multiple items from an expression source, making sure insert works as it should
        /// </summary>
        [Test]
        public void Insert02 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert:@/+/|/+/+/?node
_testX
  howdy:world
_testX
  howdy:world
pf.data.select:@/*/*/_testX/?node");
            Assert.AreEqual (2, tmp [3].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_testX", tmp [3] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual (null, tmp [3] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy", tmp [3] [0] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world", tmp [3] [0] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_testX", tmp [3] [1].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual (null, tmp [3] [1].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy", tmp [3] [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world", tmp [3] [1] [0].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts a couple of items into database, for then to perform a 'deep' update, making
        /// sure update works as it should
        /// </summary>
        [Test]
        public void Update01 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _test5
    howdy:world
  _test5
    howdy:world2
    query_field
      x:y
  _test5
    howdy:world3
pf.data.update:@/*/*/_test5/*/query_field/?node
  source
    query_field2
      x:zz
pf.data.select:@/*/*/_test5/*/query_field2/*/x/=zz/././?node");
            Assert.AreEqual (1, tmp [2].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test5", tmp [2] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world2", tmp [2] [0] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual ("x", tmp [2] [0] [1] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("zz", tmp [2] [0] [1] [0].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts some items into database, for then to update multiple items at the same time,
        /// making sure update works as it should
        /// </summary>
        [Test]
        public void Update02 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _test6
    howdy:world
  _test6
    howdy:world2
  _test6
    howdy:world3
pf.data.update:@/*/*/_test6/?node
  source
    _test6_update
      howdy:worldZZ
pf.data.select:@/*/*/_test6_update/?node
pf.data.select:@/*/*/_test6/?node");
            Assert.AreEqual (3, tmp [2].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test6_update", tmp [2] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("worldZZ", tmp [2] [0] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual ("worldZZ", tmp [2] [1] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual ("worldZZ", tmp [2] [2] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual (0, tmp [3].Count, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts one item into database, for then to update through 'value' expression, making sure update works
        /// as it should
        /// </summary>
        [Test]
        public void Update03 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _testX
pf.data.update:@/*/*/_testX/?value
  source:{0}{1}
    :hello
    :world
pf.data.select:@/*/*/_testX/?node");
            Assert.AreEqual (1, tmp [2].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_testX", tmp [2] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("helloworld", tmp [2] [0].Value, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts one item into database, for then to update root item through use of expressions,
        /// making sure update works as it should
        /// </summary>
        [Test]
        public void Update04 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _test7
    howdy:world
pf.data.update:@/*/*/_test7/?node
  source:@/./+/?node
_test7_update
  howdy2:world2
pf.data.select:@/*/*/_test7_update/?node
pf.data.select:@/*/*/_test7/?node");
            Assert.AreEqual (1, tmp [3].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test7_update", tmp [3] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy2", tmp [3] [0] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world2", tmp [3] [0] [0].Value, "wrong value of node after executing lambda object");
            Assert.AreEqual (0, tmp [4].Count, "wrong value of node after executing lambda object");
        }

        /// <summary>
        /// inserts an item into database, for them to update 'value' of item to become a node, through an expression,
        /// making sure values in database can store nodes as their values
        /// </summary>
        [Test]
        public void Update05 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _test9
    howdy:world
pf.data.update:@/*/*/_test9/*/howdy/?value
  source:@/./+/?node
_howdy:world
pf.data.select:@/*/*/_test9/?node");
            Assert.AreEqual (1, tmp [3].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test9", tmp [3] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy", tmp [3] [0] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_howdy", tmp [3] [0] [0].Get<Node> (_context).Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("world", tmp [3] [0] [0].Get<Node> (_context).Value, "wrong value of node after executing lambda object");
            Assert.AreEqual (0, tmp [3] [0] [0].Get<Node> (_context).Count, "wrong value of node after executing lambda object");
        }
        
        /// <summary>
        /// inserts an item into database, for them to update 'value' using [rel-source], making sure update works as
        /// it should
        /// </summary>
        [Test]
        public void Update06 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _test9
    howdy:world
pf.data.update:@/*/*/_test9/*/howdy/?value
  rel-source:@/./?name
pf.data.select:@/*/*/_test9/?node");
            Assert.AreEqual (1, tmp [2].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test9", tmp [2] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy", tmp [2] [0] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test9", tmp [2] [0] [0].Value, "wrong value of node after executing lambda object");
        }
        
        /// <summary>
        /// inserts an item into database, for them to update 'value' using [rel-source], where [rel-source]
        /// is a formatting expression, making sure update works as it should
        /// </summary>
        [Test]
        public void Update07 ()
        {
            Node tmp = ExecuteLambda (@"pf.data.insert
  _test9
    howdy:world
pf.data.update:@/*/*/_test9/*/howdy/?value
  rel-source:{0}{1}
    :@/./?name
    :@?value
pf.data.select:@/*/*/_test9/?node");
            Assert.AreEqual (1, tmp [2].Count, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test9", tmp [2] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("howdy", tmp [2] [0] [0].Name, "wrong value of node after executing lambda object");
            Assert.AreEqual ("_test9world", tmp [2] [0] [0].Value, "wrong value of node after executing lambda object");
        }
    }
}
