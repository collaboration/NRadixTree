/*
The MIT License

Copyright (c) 2008 Tahseen Ur Rehman, Javid Jamae

http://code.google.com/p/radixtree/

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


/**
 * Unit tests for {@link RadixTree}
 * 
 * @author Tahseen Ur Rehman (tahseen.ur.rehman {at.spam.me.not} gmail.com) 
 * @author Javid Jamae 
 * @
 */
using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RadixTree;

[TestFixture]
public class RadixTreeTest {

    RadixTree<String> trie; 
    
    [TestFixtureSetUp]
    public void createTree() {
        trie = new RadixTree<String>();
    }
    
    [Test]
    public void testSearchForPartialParentAndLeafKeyWhenOverlapExists() {
        trie.Insert("abcd", "abcd");
        trie.Insert("abce", "abce");
        
        Assert.That(trie.SearchPrefix("abe", 10).Count, Is.EqualTo(0));
        Assert.That(trie.SearchPrefix("abd", 10).Count, Is.EqualTo(0));
    }
    
    [Test]
    public void testSearchForLeafNodesWhenOverlapExists() {
        trie.Insert("abcd", "abcd");
        trie.Insert("abce", "abce");
   
        assertEquals(1, trie.SearchPrefix("abcd", 10).size());
        assertEquals(1, trie.SearchPrefix("abce", 10).size());
    }
    
    [Test]
    public void testSearchForStringSmallerThanSharedParentWhenOverlapExists() {
        trie.Insert("abcd", "abcd");
        trie.Insert("abce", "abce");
   
        assertEquals(2, trie.SearchPrefix("ab", 10).size());
        assertEquals(2, trie.SearchPrefix("a", 10).size());
    }
    
    [Test]
    public void testSearchForStringEqualToSharedParentWhenOverlapExists() {
        trie.Insert("abcd", "abcd");
        trie.Insert("abce", "abce");
   
        assertEquals(2, trie.SearchPrefix("abc", 10).size());
    }
    
    [Test]
    public void testInsert() {
        trie.Insert("apple", "apple");
        trie.Insert("bat", "bat");
        trie.Insert("ape", "ape");
        trie.Insert("bath", "bath");
        trie.Insert("banana", "banana"); 
        
        assertEquals(trie.Find("apple"), "apple");
        assertEquals(trie.Find("bat"), "bat");
        assertEquals(trie.Find("ape"), "ape");
        assertEquals(trie.Find("bath"), "bath");
        assertEquals(trie.Find("banana"), "banana");
    }
    
    [Test]
    public void testInsertExistingUnrealNodeConvertsItToReal() {
    	trie.Insert("applepie", "applepie");
    	trie.Insert("applecrisp", "applecrisp");
    	
    	assertFalse(trie.Contains("apple"));
    	
    	trie.Insert("apple", "apple");
    	
    	assertTrue(trie.Contains("apple"));
    }
    
    [Test]
    public void testDuplicatesNotAllowed() {
        RadixTree<String> trie = new RadixTree<String>();

        trie.Insert("apple", "apple");

        try {
            trie.Insert("apple", "apple2");
            fail("Duplicate should not have been allowed");
        } catch (DuplicateKeyException e) {
            assertEquals("Duplicate key: 'apple'", e.getMessage());
        }
    }
    
    [Test]
	public void testInsertWithRepeatingPatternsInKey() {
		trie.Insert("xbox 360", "xbox 360");
		trie.Insert("xbox", "xbox");
		trie.Insert("xbox 360 games", "xbox 360 games");
		trie.Insert("xbox games", "xbox games");
		trie.Insert("xbox xbox 360", "xbox xbox 360");
		trie.Insert("xbox xbox", "xbox xbox");
		trie.Insert("xbox 360 xbox games", "xbox 360 xbox games");
		trie.Insert("xbox games 360", "xbox games 360");
		trie.Insert("xbox 360 360", "xbox 360 360");
		trie.Insert("xbox 360 xbox 360", "xbox 360 xbox 360");
		trie.Insert("360 xbox games 360", "360 xbox games 360");
		trie.Insert("xbox xbox 361", "xbox xbox 361");
        
        assertEquals(12, trie.Size());
	}

    [Test]
    public void testDeleteNodeWithNoChildren() {
        RadixTree<String> trie = new RadixTree<String>();
        trie.Insert("apple", "apple");
        assertTrue(trie.Delete("apple"));
    }
    
    [Test]
    public void testDeleteNodeWithOneChild() {
        RadixTree<String> trie = new RadixTree<String>();
        trie.Insert("apple", "apple");
        trie.Insert("applepie", "applepie");
        assertTrue(trie.Delete("apple"));
        assertTrue(trie.Contains("applepie"));
        assertFalse(trie.Contains("apple"));
    }
    
    [Test]
    public void testDeleteNodeWithMultipleChildren() {
        RadixTree<String> trie = new RadixTree<String>();
        trie.Insert("apple", "apple");
        trie.Insert("applepie", "applepie");
        trie.Insert("applecrisp", "applecrisp");
        assertTrue(trie.Delete("apple"));
        assertTrue(trie.Contains("applepie"));
        assertTrue(trie.Contains("applecrisp"));
        assertFalse(trie.Contains("apple"));
    }
    
    [Test]
    public void testCantDeleteSomethingThatDoesntExist() {
        RadixTree<String> trie = new RadixTree<String>();
        assertFalse(trie.Delete("apple"));
    }

    [Test]
    public void testCantDeleteSomethingThatWasAlreadyDeleted() {
        RadixTree<String> trie = new RadixTree<String>();
        trie.Insert("apple", "apple");
        trie.Delete("apple");
        assertFalse(trie.Delete("apple"));
    }

    [Test]
    public void testChildrenNotAffectedWhenOneIsDeleted() {
        RadixTree<String> trie = new RadixTree<String>();
        trie.Insert("apple", "apple");
        trie.Insert("appleshack", "appleshack");
        trie.Insert("applepie", "applepie");
        trie.Insert("ape", "ape");
        
        trie.Delete("apple");

        assertTrue(trie.Contains("appleshack"));
        assertTrue(trie.Contains("applepie"));
        assertTrue(trie.Contains("ape"));
        assertFalse(trie.Contains("apple"));
    }
    
    [Test]
    public void testSiblingsNotAffectedWhenOneIsDeleted() {
        RadixTree<String> trie = new RadixTree<String>();
        trie.Insert("apple", "apple");
        trie.Insert("ball", "ball");
        
        trie.Delete("apple");
        
        assertTrue(trie.Contains("ball"));
    }
    
    [Test]
    public void testCantDeleteUnrealNode() {
        RadixTree<String> trie = new RadixTree<String>();
        trie.Insert("apple", "apple");
        trie.Insert("ape", "ape");
        
        assertFalse(trie.Delete("ap"));
    }
    

    [Test]
    public void testCantFindRootNode() {
        assertNull(trie.Find(""));
    }

    [Test]
    public void testFindSimpleInsert() {
        trie.Insert("apple", "apple");
        assertNotNull(trie.Find("apple"));
    }
    
    [Test]
    public void testContainsSimpleInsert() {
        trie.Insert("apple", "apple");
        assertTrue(trie.Contains("apple"));
    }

    [Test]
    public void testFindChildInsert() {
        trie.Insert("apple", "apple");
        trie.Insert("ape", "ape");
        trie.Insert("appletree", "appletree");
        trie.Insert("appleshackcream", "appleshackcream");
        assertNotNull(trie.Find("appletree"));
        assertNotNull(trie.Find("appleshackcream"));
        assertNotNull(trie.Contains("ape"));
    }
    
    [Test]
    public void testContainsChildInsert() {
        trie.Insert("apple", "apple");
        trie.Insert("ape", "ape");
        trie.Insert("appletree", "appletree");
        trie.Insert("appleshackcream", "appleshackcream");
        assertTrue(trie.Contains("appletree"));
        assertTrue(trie.Contains("appleshackcream"));
        assertTrue(trie.Contains("ape"));
    }

    [Test]
    public void testCantFindNonexistantNode() {
        assertNull(trie.Find("apple"));
    }

    [Test]
    public void testDoesntContainNonexistantNode() {
        assertFalse(trie.Contains("apple"));
    }
    
    [Test]
    public void testCantFindUnrealNode() {
        trie.Insert("apple", "apple");
        trie.Insert("ape", "ape");
        assertNull(trie.Find("ap"));
    }

    [Test]
    public void testDoesntContainUnrealNode() {
        trie.Insert("apple", "apple");
        trie.Insert("ape", "ape");
        assertFalse(trie.Contains("ap"));
    }


    [Test]
    public void testSearchPrefix_LimitGreaterThanPossibleResults() {
        trie.Insert("apple", "apple");
        trie.Insert("appleshack", "appleshack");
        trie.Insert("appleshackcream", "appleshackcream");
        trie.Insert("applepie", "applepie");
        trie.Insert("ape", "ape");

        List<String> result = trie.SearchPrefix("app", 10);
        assertEquals(4, result.size());

        assertTrue(result.Contains("appleshack"));
        assertTrue(result.Contains("appleshackcream"));
        assertTrue(result.Contains("applepie"));
        assertTrue(result.Contains("apple"));
    }
    
    [Test]
    public void testSearchPrefix_LimitLessThanPossibleResults() {
        trie.Insert("apple", "apple");
        trie.Insert("appleshack", "appleshack");
        trie.Insert("appleshackcream", "appleshackcream");
        trie.Insert("applepie", "applepie");
        trie.Insert("ape", "ape");

        List<String> result = trie.SearchPrefix("appl", 3);
        assertEquals(3, result.size());

        assertTrue(result.Contains("appleshack"));
        assertTrue(result.Contains("applepie"));
        assertTrue(result.Contains("apple"));
    }

    [Test]
    public void testGetSize() {
        trie.Insert("apple", "apple");
        trie.Insert("appleshack", "appleshack");
        trie.Insert("appleshackcream", "appleshackcream");
        trie.Insert("applepie", "applepie");
        trie.Insert("ape", "ape");
        
        assertTrue(trie.getSize() == 5);
    }
    
    [Test]
    public void testDeleteReducesSize() {
        trie.Insert("apple", "apple");
        trie.Insert("appleshack", "appleshack");
        
        trie.Delete("appleshack");
        
        assertTrue(trie.getSize() == 1);
    }    
    
    [Test]
    public void testComplete() {
    	// create a new Trie
    	RadixTree<String> trie = new RadixTree<String>();
    	
        trie.Insert("apple", "apple");
        trie.Insert("appleshack", "appleshack");
        trie.Insert("applepie", "applepie");
        trie.Insert("applegold", "applegold");
        trie.Insert("applegood", "applegood");
        
        assertEquals("", trie.Complete("z"));
        assertEquals("apple", trie.Complete("a"));
        assertEquals("apple", trie.Complete("app"));
        assertEquals("appleshack", trie.Complete("apples"));
        assertEquals("applego", trie.Complete("appleg"));
    }
}
