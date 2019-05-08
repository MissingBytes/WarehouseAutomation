using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewEditModeTest {

	[Test]
	public void NewEditModeTestSimplePasses() {

        int[,] matrix = new int[10, 20] {{ 0,0,0,0,0, 0,0,0,0,0 ,0,0,0,0,0, 0,0,0,0,0} ,
                                         { 0,0,0,0,0, 0,0,0,0,0 ,0,0,0,0,0, 0,0,0,0,0} ,
                                         { 0,0,0,0,0, 0,0,0,0,0 ,0,0,0,0,0, 0,0,0,0,0} ,
                                         { 0,0,0,0,0, 0,0,0,0,0 ,0,0,0,0,0, 0,0,0,0,0} ,
                                         { 0,0,0,0,1, 1,1,0,0,0 ,0,0,1,1,0, 0,0,0,1,1} ,
                                         { 1,1,0,1,1, 1,1,1,1,1 ,1,1,1,1,0, 0,0,0,1,1} ,
                                         { 1,1,0,1,1, 1,1,1,1,1 ,1,1,1,1,0, 0,0,0,1,1} ,
                                         { 1,1,1,1,1, 1,1,1,1,1 ,1,1,1,1,1, 1,1,1,1,1} ,
                                         { 1,1,1,1,1, 1,1,1,1,1 ,1,1,1,1,1, 1,1,1,1,1} ,
                                         { 1,1,1,1,1, 1,1,1,1,1 ,1,1,1,1,1, 1,1,1,1,1} };
        // Use the Assert class to test conditions.

       
        var CRB = new CreateRandomBoxes();
        var agg_height = CRB.Aggregate_height(matrix, 1);

        Assert.That(97, Is.EqualTo(agg_height));
    }

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator NewEditModeTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
