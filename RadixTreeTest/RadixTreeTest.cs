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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RadixTree;


namespace RadixTreeTest
{
    [TestFixture]
    public class RadixTreeTest {

        RadixTree<String> trie; 
    
        [TestFixtureSetUp]
        public void CreateTree() {
            trie = new RadixTree<String>();
        }
    
        [Test]
        public void TestSearchForPartialParentAndLeafKeyWhenOverlapExists() {
            trie.Insert("abcd", "abcd");
            trie.Insert("abce", "abce");
        
            Assert.That(trie.SearchPrefix("abe", 10).Count, Is.EqualTo(0));
            Assert.That(trie.SearchPrefix("abd", 10).Count, Is.EqualTo(0));
        }
    
        [Test]
        public void TestSearchForLeafNodesWhenOverlapExists() {
            trie.Insert("abcd", "abcd");
            trie.Insert("abce", "abce");
   
            Assert.That(trie.SearchPrefix("abcd", 10).Count, Is.EqualTo(1));
            Assert.That(trie.SearchPrefix("abce", 10).Count, Is.EqualTo(1));
        }
    
        [Test]
        public void TestSearchForStringSmallerThanSharedParentWhenOverlapExists() {
            trie.Insert("abcd", "abcd");
            trie.Insert("abce", "abce");

            Assert.That(trie.SearchPrefix("ab", 10).Count, Is.EqualTo(2));
            Assert.That(trie.SearchPrefix("a", 10).Count, Is.EqualTo(2));
        }
    
        [Test]
        public void TestSearchForStringEqualToSharedParentWhenOverlapExists() {
            trie.Insert("abcd", "abcd");
            trie.Insert("abce", "abce");

            Assert.That(trie.SearchPrefix("abc", 10).Count, Is.EqualTo(2));
        }
    
        [Test]
        public void TestInsert() {
            trie.Insert("apple", "apple");
            trie.Insert("bat", "bat");
            trie.Insert("ape", "ape");
            trie.Insert("bath", "bath");
            trie.Insert("banana", "banana"); 
        
            Assert.That(trie.Find("apple"), Is.EqualTo("apple"));
            Assert.That(trie.Find("bat"), Is.EqualTo("bat"));
            Assert.That(trie.Find("ape"), Is.EqualTo("ape"));
            Assert.That(trie.Find("bath"), Is.EqualTo("bath"));
            Assert.That(trie.Find("banana"), Is.EqualTo("banana"));
        }
    
        [Test]
        public void TestInsertExistingUnrealNodeConvertsItToReal() {
            trie.Insert("applepie", "applepie");
            trie.Insert("applecrisp", "applecrisp");
    	
            Assert.That(trie.Contains("apple"), Is.False);
    	
            trie.Insert("apple", "apple");
    	
            Assert.That(trie.Contains("apple"));
        }
    
        [Test]
        public void TestDuplicatesNotAllowed() {
            var trie = new RadixTree<String>();

            trie.Insert("apple", "apple");

            try {
                trie.Insert("apple", "apple2");
                Assert.Fail("Duplicate should not have been allowed");
            } catch (DuplicateKeyException e) {
                Assert.That(e.Message, Is.EqualTo("Duplicate key: 'apple'"));
            }
        }
    
        [Test]
        public void TestInsertWithRepeatingPatternsInKey() {
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
        
            Assert.That(trie.Size(), Is.EqualTo(12));
        }

        [Test]
        public void TestDeleteNodeWithNoChildren() {
            var trie = new RadixTree<String>();
            trie.Insert("apple", "apple");
            Assert.That(trie.Delete("apple"));
        }
    
        [Test]
        public void TestDeleteNodeWithOneChild() {
            var trie = new RadixTree<String>();
            trie.Insert("apple", "apple");
            trie.Insert("applepie", "applepie");
            Assert.That(trie.Delete("apple"));
            Assert.That(trie.Contains("applepie"));
            Assert.That(trie.Contains("apple"));
        }
    
        [Test]
        public void TestDeleteNodeWithMultipleChildren() {
            var trie = new RadixTree<String>();
            trie.Insert("apple", "apple");
            trie.Insert("applepie", "applepie");
            trie.Insert("applecrisp", "applecrisp");
            Assert.That(trie.Delete("apple"));
            Assert.That(trie.Contains("applepie"));
            Assert.That(trie.Contains("applecrisp"));
            Assert.That(trie.Contains("apple"), Is.False);
        }
    
        [Test]
        public void TestCantDeleteSomethingThatDoesntExist() {
            var trie = new RadixTree<String>();
            Assert.That(trie.Delete("apple"), Is.False);
        }

        [Test]
        public void TestCantDeleteSomethingThatWasAlreadyDeleted() {
            var trie = new RadixTree<String>();
            trie.Insert("apple", "apple");
            trie.Delete("apple");
            Assert.That(trie.Delete("apple"), Is.False);
        }

        [Test]
        public void TestChildrenNotAffectedWhenOneIsDeleted() {
            var trie = new RadixTree<String>();
            trie.Insert("apple", "apple");
            trie.Insert("appleshack", "appleshack");
            trie.Insert("applepie", "applepie");
            trie.Insert("ape", "ape");
        
            trie.Delete("apple");

            Assert.That(trie.Contains("appleshack"));
            Assert.That(trie.Contains("applepie"));
            Assert.That(trie.Contains("ape"));
            Assert.That(trie.Contains("apple"), Is.False);
        }
    
        [Test]
        public void TestSiblingsNotAffectedWhenOneIsDeleted() {
            var trie = new RadixTree<String>();
            trie.Insert("apple", "apple");
            trie.Insert("ball", "ball");
        
            trie.Delete("apple");

            Assert.That(trie.Contains("ball"));
        }
    
        [Test]
        public void TestCantDeleteUnrealNode() {
            var trie = new RadixTree<String>();
            trie.Insert("apple", "apple");
            trie.Insert("ape", "ape");

            Assert.That(trie.Delete("ap"), Is.False);
        }
    

        [Test]
        public void TestCantFindRootNode() {
            Assert.That(trie.Find(""), Is.Null);
        }

        [Test]
        public void TestFindSimpleInsert() {
            trie.Insert("apple", "apple");
            Assert.That(trie.Find("apple"), Is.Not.Null);
        }
    
        [Test]
        public void TestContainsSimpleInsert() {
            trie.Insert("apple", "apple");
            Assert.That(trie.Contains("apple"));
        }

        [Test]
        public void TestFindChildInsert() {
            trie.Insert("apple", "apple");
            trie.Insert("ape", "ape");
            trie.Insert("appletree", "appletree");
            trie.Insert("appleshackcream", "appleshackcream");
            Assert.That(trie.Find("appletree"), Is.Not.Null);
            Assert.That(trie.Find("appleshackcream"), Is.Not.Null);
            Assert.That(trie.Contains("ape"), Is.Not.Null);
        }
    
        [Test]
        public void TestContainsChildInsert() {
            trie.Insert("apple", "apple");
            trie.Insert("ape", "ape");
            trie.Insert("appletree", "appletree");
            trie.Insert("appleshackcream", "appleshackcream");
            Assert.That(trie.Contains("appletree"));
            Assert.That(trie.Contains("appleshackcream"));
            Assert.That(trie.Contains("ape"));
        }

        [Test]
        public void TestCantFindNonexistantNode() {
            Assert.That(trie.Find("apple"), Is.Not.Null);
        }

        [Test]
        public void TestDoesntContainNonexistantNode() {
            Assert.That(trie.Contains("apple"), Is.False);
        }
    
        [Test]
        public void TestCantFindUnrealNode() {
            trie.Insert("apple", "apple");
            trie.Insert("ape", "ape");
            Assert.That(trie.Find("ap"), Is.Null);
        }

        [Test]
        public void TestDoesntContainUnrealNode() {
            trie.Insert("apple", "apple");
            trie.Insert("ape", "ape");
            Assert.That(trie.Contains("ap"), Is.False);
        }


        [Test]
        public void TestSearchPrefixLimitGreaterThanPossibleResults() {
            trie.Insert("apple", "apple");
            trie.Insert("appleshack", "appleshack");
            trie.Insert("appleshackcream", "appleshackcream");
            trie.Insert("applepie", "applepie");
            trie.Insert("ape", "ape");

            var result = trie.SearchPrefix("app", 10);
            Assert.That(result.Count, Is.EqualTo(4));

            Assert.That(result.Contains("appleshack"));
            Assert.That(result.Contains("appleshackcream"));
            Assert.That(result.Contains("applepie"));
            Assert.That(result.Contains("apple"));
        }
    
        [Test]
        public void TestSearchPrefixLimitLessThanPossibleResults() {
            trie.Insert("apple", "apple");
            trie.Insert("appleshack", "appleshack");
            trie.Insert("appleshackcream", "appleshackcream");
            trie.Insert("applepie", "applepie");
            trie.Insert("ape", "ape");

            var result = trie.SearchPrefix("appl", 3);
            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result.Contains("appleshack"));
            Assert.That(result.Contains("applepie"));
            Assert.That(result.Contains("apple"));
        }

        [Test]
        public void TestGetSize() {
            trie.Insert("apple", "apple");
            trie.Insert("appleshack", "appleshack");
            trie.Insert("appleshackcream", "appleshackcream");
            trie.Insert("applepie", "applepie");
            trie.Insert("ape", "ape");

            Assert.That(trie.Size(), Is.EqualTo(5));
        }
    
        [Test]
        public void TestDeleteReducesSize() {
            trie.Insert("apple", "apple");
            trie.Insert("appleshack", "appleshack");
        
            trie.Delete("appleshack");

            Assert.That(trie.Size(), Is.EqualTo(1));
        }    
    
        [Test]
        public void TestComplete() {
            var trie = new RadixTree<String>();
    	
            trie.Insert("apple", "apple");
            trie.Insert("appleshack", "appleshack");
            trie.Insert("applepie", "applepie");
            trie.Insert("applegold", "applegold");
            trie.Insert("applegood", "applegood");
        
            Assert.That(trie.Complete("z"), Is.EqualTo(""));
            Assert.That(trie.Complete("a"), Is.EqualTo("apple"));
            Assert.That(trie.Complete("app"), Is.EqualTo("apple"));
            Assert.That(trie.Complete("apples"), Is.EqualTo("appleshack"));
            Assert.That(trie.Complete("appleg"), Is.EqualTo("applego"));
        }
    }
}