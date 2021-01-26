using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using StardustCore.UIUtilities.SpriteFonts.Components;

namespace StardustCore.UIUtilities.SpriteFonts.CharacterSheets
{
    public class VanillaCharacterSheet : GenericCharacterSheets
    {

        public VanillaCharacterSheet(string directoryToFonts)
        {
            Color fontColor = StardustCore.IlluminateFramework.Colors.GetColorFromList("Black");
            this.CharacterAtlus = new Dictionary<char, TexturedCharacter>();
            string path = directoryToFonts;
            this.CharacterAtlus.Add('0', new TexturedCharacter('0', Path.Combine(directoryToFonts, "0"), fontColor));
            this.CharacterAtlus.Add('1', new TexturedCharacter('1', Path.Combine(directoryToFonts, "1"), fontColor));
            this.CharacterAtlus.Add('2', new TexturedCharacter('2', Path.Combine(directoryToFonts, "2"), fontColor));
            this.CharacterAtlus.Add('3', new TexturedCharacter('3', Path.Combine(directoryToFonts, "3"), fontColor));
            this.CharacterAtlus.Add('4', new TexturedCharacter('4', Path.Combine(directoryToFonts, "4"), fontColor));
            this.CharacterAtlus.Add('5', new TexturedCharacter('5', Path.Combine(directoryToFonts, "5"), fontColor));
            this.CharacterAtlus.Add('6', new TexturedCharacter('6', Path.Combine(directoryToFonts, "6"), fontColor));
            this.CharacterAtlus.Add('7', new TexturedCharacter('7', Path.Combine(directoryToFonts, "7"), fontColor));
            this.CharacterAtlus.Add('8', new TexturedCharacter('8', Path.Combine(directoryToFonts, "8"), fontColor));
            this.CharacterAtlus.Add('9', new TexturedCharacter('9', Path.Combine(directoryToFonts, "9"), fontColor));

            this.CharacterAtlus.Add('&', new TexturedCharacter('&', Path.Combine(directoryToFonts, "ampersand"), fontColor));
            this.CharacterAtlus.Add('*', new TexturedCharacter('*', Path.Combine(directoryToFonts, "asterisk"), fontColor));
            this.CharacterAtlus.Add('\\', new TexturedCharacter('\\', Path.Combine(directoryToFonts, "backSlash"), fontColor));

            this.CharacterAtlus.Add('A', new TexturedCharacter('A', Path.Combine(directoryToFonts, "capitalA"), fontColor));
            this.CharacterAtlus.Add('B', new TexturedCharacter('B', Path.Combine(directoryToFonts, "capitalB"), fontColor));
            this.CharacterAtlus.Add('C', new TexturedCharacter('C', Path.Combine(directoryToFonts, "capitalC"), fontColor));
            this.CharacterAtlus.Add('D', new TexturedCharacter('D', Path.Combine(directoryToFonts, "capitalD"), fontColor));
            this.CharacterAtlus.Add('E', new TexturedCharacter('E', Path.Combine(directoryToFonts, "capitalE"), fontColor));
            this.CharacterAtlus.Add('F', new TexturedCharacter('F', Path.Combine(directoryToFonts, "capitalF"), fontColor));
            this.CharacterAtlus.Add('G', new TexturedCharacter('G', Path.Combine(directoryToFonts, "capitalG"), fontColor));
            this.CharacterAtlus.Add('H', new TexturedCharacter('H', Path.Combine(directoryToFonts, "capitalH"), fontColor));
            this.CharacterAtlus.Add('I', new TexturedCharacter('I', Path.Combine(directoryToFonts, "capitalI"), fontColor));
            this.CharacterAtlus.Add('J', new TexturedCharacter('J', Path.Combine(directoryToFonts, "capitalJ"), fontColor));
            this.CharacterAtlus.Add('K', new TexturedCharacter('K', Path.Combine(directoryToFonts, "capitalK"), fontColor));
            this.CharacterAtlus.Add('L', new TexturedCharacter('L', Path.Combine(directoryToFonts, "capitalL"), fontColor));
            this.CharacterAtlus.Add('M', new TexturedCharacter('M', Path.Combine(directoryToFonts, "capitalM"), fontColor));
            this.CharacterAtlus.Add('N', new TexturedCharacter('N', Path.Combine(directoryToFonts, "capitalN"), fontColor));
            this.CharacterAtlus.Add('O', new TexturedCharacter('O', Path.Combine(directoryToFonts, "capitalO"), fontColor));
            this.CharacterAtlus.Add('P', new TexturedCharacter('P', Path.Combine(directoryToFonts, "capitalP"), fontColor));
            this.CharacterAtlus.Add('Q', new TexturedCharacter('Q', Path.Combine(directoryToFonts, "capitalQ"), fontColor));
            this.CharacterAtlus.Add('R', new TexturedCharacter('R', Path.Combine(directoryToFonts, "capitalR"), fontColor));
            this.CharacterAtlus.Add('S', new TexturedCharacter('S', Path.Combine(directoryToFonts, "capitalS"), fontColor));
            this.CharacterAtlus.Add('T', new TexturedCharacter('T', Path.Combine(directoryToFonts, "capitalT"), fontColor));
            this.CharacterAtlus.Add('U', new TexturedCharacter('U', Path.Combine(directoryToFonts, "capitalU"), fontColor));
            this.CharacterAtlus.Add('V', new TexturedCharacter('V', Path.Combine(directoryToFonts, "capitalV"), fontColor));
            this.CharacterAtlus.Add('W', new TexturedCharacter('W', Path.Combine(directoryToFonts, "capitalW"), fontColor));
            this.CharacterAtlus.Add('X', new TexturedCharacter('X', Path.Combine(directoryToFonts, "capitalX"), fontColor));
            this.CharacterAtlus.Add('Y', new TexturedCharacter('Y', Path.Combine(directoryToFonts, "capitalY"), fontColor));
            this.CharacterAtlus.Add('Z', new TexturedCharacter('Z', Path.Combine(directoryToFonts, "capitalZ"), fontColor));

            this.CharacterAtlus.Add('^', new TexturedCharacter('^', Path.Combine(directoryToFonts, "caret"), fontColor));
            this.CharacterAtlus.Add(':', new TexturedCharacter(':', Path.Combine(directoryToFonts, "colon"), fontColor));
            this.CharacterAtlus.Add(',', new TexturedCharacter(',', Path.Combine(directoryToFonts, "comma"), fontColor));
            this.CharacterAtlus.Add('\"', new TexturedCharacter('\"', Path.Combine(directoryToFonts, "doubleQuotes"), fontColor));
            this.CharacterAtlus.Add('!', new TexturedCharacter('!', Path.Combine(directoryToFonts, "exclamationMark"), fontColor));
            this.CharacterAtlus.Add('/', new TexturedCharacter('/', Path.Combine(directoryToFonts, "forwardSlash"), fontColor));
            this.CharacterAtlus.Add('`', new TexturedCharacter('`', Path.Combine(directoryToFonts, "grave"), fontColor));
            this.CharacterAtlus.Add('[', new TexturedCharacter('[', Path.Combine(directoryToFonts, "leftBracket"), fontColor));
            this.CharacterAtlus.Add('{', new TexturedCharacter('{', Path.Combine(directoryToFonts, "leftCurlyBracket"), fontColor));
            this.CharacterAtlus.Add('(', new TexturedCharacter('(', Path.Combine(directoryToFonts, "leftParenthesis"), fontColor));

            this.CharacterAtlus.Add('a', new TexturedCharacter('a', Path.Combine(directoryToFonts, "lowercaseA"), fontColor));
            this.CharacterAtlus.Add('b', new TexturedCharacter('b', Path.Combine(directoryToFonts, "lowercaseB"), fontColor));
            this.CharacterAtlus.Add('c', new TexturedCharacter('c', Path.Combine(directoryToFonts, "lowercaseC"), fontColor));
            this.CharacterAtlus.Add('d', new TexturedCharacter('d', Path.Combine(directoryToFonts, "lowercaseD"), fontColor));
            this.CharacterAtlus.Add('e', new TexturedCharacter('e', Path.Combine(directoryToFonts, "lowercaseE"), fontColor));
            this.CharacterAtlus.Add('f', new TexturedCharacter('f', Path.Combine(directoryToFonts, "lowercaseF"), fontColor));
            this.CharacterAtlus.Add('g', new TexturedCharacter('g', Path.Combine(directoryToFonts, "lowercaseG"), fontColor));
            this.CharacterAtlus.Add('h', new TexturedCharacter('h', Path.Combine(directoryToFonts, "lowercaseH"), fontColor));
            this.CharacterAtlus.Add('i', new TexturedCharacter('i', Path.Combine(directoryToFonts, "lowercaseI"), fontColor));
            this.CharacterAtlus.Add('j', new TexturedCharacter('j', Path.Combine(directoryToFonts, "lowercaseJ"), fontColor));
            this.CharacterAtlus.Add('k', new TexturedCharacter('k', Path.Combine(directoryToFonts, "lowercaseK"), fontColor));
            this.CharacterAtlus.Add('l', new TexturedCharacter('l', Path.Combine(directoryToFonts, "lowercaseL"), fontColor));
            this.CharacterAtlus.Add('m', new TexturedCharacter('m', Path.Combine(directoryToFonts, "lowercaseM"), fontColor));
            this.CharacterAtlus.Add('n', new TexturedCharacter('n', Path.Combine(directoryToFonts, "lowercaseN"), fontColor));
            this.CharacterAtlus.Add('o', new TexturedCharacter('o', Path.Combine(directoryToFonts, "lowercaseO"), fontColor));
            this.CharacterAtlus.Add('p', new TexturedCharacter('p', Path.Combine(directoryToFonts, "lowercaseP"), fontColor));
            this.CharacterAtlus.Add('q', new TexturedCharacter('q', Path.Combine(directoryToFonts, "lowercaseQ"), fontColor));
            this.CharacterAtlus.Add('r', new TexturedCharacter('r', Path.Combine(directoryToFonts, "lowercaseR"), fontColor));
            this.CharacterAtlus.Add('s', new TexturedCharacter('s', Path.Combine(directoryToFonts, "lowercaseS"), fontColor));
            this.CharacterAtlus.Add('t', new TexturedCharacter('t', Path.Combine(directoryToFonts, "lowercaseT"), fontColor));
            this.CharacterAtlus.Add('u', new TexturedCharacter('u', Path.Combine(directoryToFonts, "lowercaseU"), fontColor));
            this.CharacterAtlus.Add('v', new TexturedCharacter('v', Path.Combine(directoryToFonts, "lowercaseV"), fontColor));
            this.CharacterAtlus.Add('w', new TexturedCharacter('w', Path.Combine(directoryToFonts, "lowercaseW"), fontColor));
            this.CharacterAtlus.Add('x', new TexturedCharacter('x', Path.Combine(directoryToFonts, "lowercaseX"), fontColor));
            this.CharacterAtlus.Add('y', new TexturedCharacter('y', Path.Combine(directoryToFonts, "lowercaseY"), fontColor));
            this.CharacterAtlus.Add('z', new TexturedCharacter('z', Path.Combine(directoryToFonts, "lowercaseZ"), fontColor));

            this.CharacterAtlus.Add('-', new TexturedCharacter('-', Path.Combine(directoryToFonts, "minus"), fontColor));
            this.CharacterAtlus.Add('%', new TexturedCharacter('%', Path.Combine(directoryToFonts, "percent"), fontColor));
            this.CharacterAtlus.Add('.', new TexturedCharacter('.', Path.Combine(directoryToFonts, "period"), fontColor));
            this.CharacterAtlus.Add('+', new TexturedCharacter('+', Path.Combine(directoryToFonts, "plus"), fontColor));
            this.CharacterAtlus.Add('#', new TexturedCharacter('#', Path.Combine(directoryToFonts, "pound"), fontColor));

            this.CharacterAtlus.Add('?', new TexturedCharacter('?', Path.Combine(directoryToFonts, "questionMark"), fontColor));
            this.CharacterAtlus.Add(']', new TexturedCharacter(']', Path.Combine(directoryToFonts, "rightBracket"), fontColor));
            this.CharacterAtlus.Add('}', new TexturedCharacter('}', Path.Combine(directoryToFonts, "rightCurlyBracket"), fontColor));

            this.CharacterAtlus.Add(')', new TexturedCharacter(')', Path.Combine(directoryToFonts, "rightParenthesis"), fontColor));

            this.CharacterAtlus.Add(';', new TexturedCharacter(';', Path.Combine(directoryToFonts, "semicolon"), fontColor));

            this.CharacterAtlus.Add('\'', new TexturedCharacter('\'', Path.Combine(directoryToFonts, "singleQuote"), fontColor));
            this.CharacterAtlus.Add(' ', new TexturedCharacter(' ', Path.Combine(directoryToFonts, "space"), fontColor));
            this.CharacterAtlus.Add('~', new TexturedCharacter('~', Path.Combine(directoryToFonts, "tilde"), fontColor));
            this.CharacterAtlus.Add('_', new TexturedCharacter('_', Path.Combine(directoryToFonts, "underScore"), fontColor));
            this.CharacterAtlus.Add('|', new TexturedCharacter('|', Path.Combine(directoryToFonts, "verticalLine"), fontColor));

            this.CharacterAtlus.Add('$', new TexturedCharacter('$', Path.Combine(directoryToFonts, "coin"), fontColor));
            this.CharacterAtlus.Add('=', new TexturedCharacter('=', Path.Combine(directoryToFonts, "star"), fontColor));
            this.CharacterAtlus.Add('@', new TexturedCharacter('@', Path.Combine(directoryToFonts, "heart"), fontColor));
        }

        public VanillaCharacterSheet(string directoryToFonts, Color fontColor)
        {
            this.CharacterAtlus = new Dictionary<char, TexturedCharacter>();
            this.CharacterAtlus.Add('0', new TexturedCharacter('0', Path.Combine(directoryToFonts, "0"), fontColor));
            this.CharacterAtlus.Add('1', new TexturedCharacter('1', Path.Combine(directoryToFonts, "1"), fontColor));
            this.CharacterAtlus.Add('2', new TexturedCharacter('2', Path.Combine(directoryToFonts, "2"), fontColor));
            this.CharacterAtlus.Add('3', new TexturedCharacter('3', Path.Combine(directoryToFonts, "3"), fontColor));
            this.CharacterAtlus.Add('4', new TexturedCharacter('4', Path.Combine(directoryToFonts, "4"), fontColor));
            this.CharacterAtlus.Add('5', new TexturedCharacter('5', Path.Combine(directoryToFonts, "5"), fontColor));
            this.CharacterAtlus.Add('6', new TexturedCharacter('6', Path.Combine(directoryToFonts, "6"), fontColor));
            this.CharacterAtlus.Add('7', new TexturedCharacter('7', Path.Combine(directoryToFonts, "7"), fontColor));
            this.CharacterAtlus.Add('8', new TexturedCharacter('8', Path.Combine(directoryToFonts, "8"), fontColor));
            this.CharacterAtlus.Add('9', new TexturedCharacter('9', Path.Combine(directoryToFonts, "9"), fontColor));

            this.CharacterAtlus.Add('&', new TexturedCharacter('&', Path.Combine(directoryToFonts, "ampersand"), fontColor));
            this.CharacterAtlus.Add('*', new TexturedCharacter('*', Path.Combine(directoryToFonts, "asterisk"), fontColor));
            this.CharacterAtlus.Add('\\', new TexturedCharacter('\\', Path.Combine(directoryToFonts, "backSlash"), fontColor));

            this.CharacterAtlus.Add('A', new TexturedCharacter('A', Path.Combine(directoryToFonts, "capitalA"), fontColor));
            this.CharacterAtlus.Add('B', new TexturedCharacter('B', Path.Combine(directoryToFonts, "capitalB"), fontColor));
            this.CharacterAtlus.Add('C', new TexturedCharacter('C', Path.Combine(directoryToFonts, "capitalC"), fontColor));
            this.CharacterAtlus.Add('D', new TexturedCharacter('D', Path.Combine(directoryToFonts, "capitalD"), fontColor));
            this.CharacterAtlus.Add('E', new TexturedCharacter('E', Path.Combine(directoryToFonts, "capitalE"), fontColor));
            this.CharacterAtlus.Add('F', new TexturedCharacter('F', Path.Combine(directoryToFonts, "capitalF"), fontColor));
            this.CharacterAtlus.Add('G', new TexturedCharacter('G', Path.Combine(directoryToFonts, "capitalG"), fontColor));
            this.CharacterAtlus.Add('H', new TexturedCharacter('H', Path.Combine(directoryToFonts, "capitalH"), fontColor));
            this.CharacterAtlus.Add('I', new TexturedCharacter('I', Path.Combine(directoryToFonts, "capitalI"), fontColor));
            this.CharacterAtlus.Add('J', new TexturedCharacter('J', Path.Combine(directoryToFonts, "capitalJ"), fontColor));
            this.CharacterAtlus.Add('K', new TexturedCharacter('K', Path.Combine(directoryToFonts, "capitalK"), fontColor));
            this.CharacterAtlus.Add('L', new TexturedCharacter('L', Path.Combine(directoryToFonts, "capitalL"), fontColor));
            this.CharacterAtlus.Add('M', new TexturedCharacter('M', Path.Combine(directoryToFonts, "capitalM"), fontColor));
            this.CharacterAtlus.Add('N', new TexturedCharacter('N', Path.Combine(directoryToFonts, "capitalN"), fontColor));
            this.CharacterAtlus.Add('O', new TexturedCharacter('O', Path.Combine(directoryToFonts, "capitalO"), fontColor));
            this.CharacterAtlus.Add('P', new TexturedCharacter('P', Path.Combine(directoryToFonts, "capitalP"), fontColor));
            this.CharacterAtlus.Add('Q', new TexturedCharacter('Q', Path.Combine(directoryToFonts, "capitalQ"), fontColor));
            this.CharacterAtlus.Add('R', new TexturedCharacter('R', Path.Combine(directoryToFonts, "capitalR"), fontColor));
            this.CharacterAtlus.Add('S', new TexturedCharacter('S', Path.Combine(directoryToFonts, "capitalS"), fontColor));
            this.CharacterAtlus.Add('T', new TexturedCharacter('T', Path.Combine(directoryToFonts, "capitalT"), fontColor));
            this.CharacterAtlus.Add('U', new TexturedCharacter('U', Path.Combine(directoryToFonts, "capitalU"), fontColor));
            this.CharacterAtlus.Add('V', new TexturedCharacter('V', Path.Combine(directoryToFonts, "capitalV"), fontColor));
            this.CharacterAtlus.Add('W', new TexturedCharacter('W', Path.Combine(directoryToFonts, "capitalW"), fontColor));
            this.CharacterAtlus.Add('X', new TexturedCharacter('X', Path.Combine(directoryToFonts, "capitalX"), fontColor));
            this.CharacterAtlus.Add('Y', new TexturedCharacter('Y', Path.Combine(directoryToFonts, "capitalY"), fontColor));
            this.CharacterAtlus.Add('Z', new TexturedCharacter('Z', Path.Combine(directoryToFonts, "capitalZ"), fontColor));

            this.CharacterAtlus.Add('^', new TexturedCharacter('^', Path.Combine(directoryToFonts, "caret"), fontColor));
            this.CharacterAtlus.Add(':', new TexturedCharacter(':', Path.Combine(directoryToFonts, "colon"), fontColor));
            this.CharacterAtlus.Add(',', new TexturedCharacter(',', Path.Combine(directoryToFonts, "comma"), fontColor));
            this.CharacterAtlus.Add('\"', new TexturedCharacter('\"', Path.Combine(directoryToFonts, "doubleQuotes"), fontColor));
            this.CharacterAtlus.Add('!', new TexturedCharacter('!', Path.Combine(directoryToFonts, "exclamationMark"), fontColor));
            this.CharacterAtlus.Add('/', new TexturedCharacter('/', Path.Combine(directoryToFonts, "forwardSlash"), fontColor));
            this.CharacterAtlus.Add('`', new TexturedCharacter('`', Path.Combine(directoryToFonts, "grave"), fontColor));
            this.CharacterAtlus.Add('[', new TexturedCharacter('[', Path.Combine(directoryToFonts, "leftBracket"), fontColor));
            this.CharacterAtlus.Add('{', new TexturedCharacter('{', Path.Combine(directoryToFonts, "leftCurlyBracket"), fontColor));
            this.CharacterAtlus.Add('(', new TexturedCharacter('(', Path.Combine(directoryToFonts, "leftParenthesis"), fontColor));

            this.CharacterAtlus.Add('a', new TexturedCharacter('a', Path.Combine(directoryToFonts, "lowercaseA"), fontColor));
            this.CharacterAtlus.Add('b', new TexturedCharacter('b', Path.Combine(directoryToFonts, "lowercaseB"), fontColor));
            this.CharacterAtlus.Add('c', new TexturedCharacter('c', Path.Combine(directoryToFonts, "lowercaseC"), fontColor));
            this.CharacterAtlus.Add('d', new TexturedCharacter('d', Path.Combine(directoryToFonts, "lowercaseD"), fontColor));
            this.CharacterAtlus.Add('e', new TexturedCharacter('e', Path.Combine(directoryToFonts, "lowercaseE"), fontColor));
            this.CharacterAtlus.Add('f', new TexturedCharacter('f', Path.Combine(directoryToFonts, "lowercaseF"), fontColor));
            this.CharacterAtlus.Add('g', new TexturedCharacter('g', Path.Combine(directoryToFonts, "lowercaseG"), fontColor));
            this.CharacterAtlus.Add('h', new TexturedCharacter('h', Path.Combine(directoryToFonts, "lowercaseH"), fontColor));
            this.CharacterAtlus.Add('i', new TexturedCharacter('i', Path.Combine(directoryToFonts, "lowercaseI"), fontColor));
            this.CharacterAtlus.Add('j', new TexturedCharacter('j', Path.Combine(directoryToFonts, "lowercaseJ"), fontColor));
            this.CharacterAtlus.Add('k', new TexturedCharacter('k', Path.Combine(directoryToFonts, "lowercaseK"), fontColor));
            this.CharacterAtlus.Add('l', new TexturedCharacter('l', Path.Combine(directoryToFonts, "lowercaseL"), fontColor));
            this.CharacterAtlus.Add('m', new TexturedCharacter('m', Path.Combine(directoryToFonts, "lowercaseM"), fontColor));
            this.CharacterAtlus.Add('n', new TexturedCharacter('n', Path.Combine(directoryToFonts, "lowercaseN"), fontColor));
            this.CharacterAtlus.Add('o', new TexturedCharacter('o', Path.Combine(directoryToFonts, "lowercaseO"), fontColor));
            this.CharacterAtlus.Add('p', new TexturedCharacter('p', Path.Combine(directoryToFonts, "lowercaseP"), fontColor));
            this.CharacterAtlus.Add('q', new TexturedCharacter('q', Path.Combine(directoryToFonts, "lowercaseQ"), fontColor));
            this.CharacterAtlus.Add('r', new TexturedCharacter('r', Path.Combine(directoryToFonts, "lowercaseR"), fontColor));
            this.CharacterAtlus.Add('s', new TexturedCharacter('s', Path.Combine(directoryToFonts, "lowercaseS"), fontColor));
            this.CharacterAtlus.Add('t', new TexturedCharacter('t', Path.Combine(directoryToFonts, "lowercaseT"), fontColor));
            this.CharacterAtlus.Add('u', new TexturedCharacter('u', Path.Combine(directoryToFonts, "lowercaseU"), fontColor));
            this.CharacterAtlus.Add('v', new TexturedCharacter('v', Path.Combine(directoryToFonts, "lowercaseV"), fontColor));
            this.CharacterAtlus.Add('w', new TexturedCharacter('w', Path.Combine(directoryToFonts, "lowercaseW"), fontColor));
            this.CharacterAtlus.Add('x', new TexturedCharacter('x', Path.Combine(directoryToFonts, "lowercaseX"), fontColor));
            this.CharacterAtlus.Add('y', new TexturedCharacter('y', Path.Combine(directoryToFonts, "lowercaseY"), fontColor));
            this.CharacterAtlus.Add('z', new TexturedCharacter('z', Path.Combine(directoryToFonts, "lowercaseZ"), fontColor));

            this.CharacterAtlus.Add('-', new TexturedCharacter('-', Path.Combine(directoryToFonts, "minus"), fontColor));
            this.CharacterAtlus.Add('%', new TexturedCharacter('%', Path.Combine(directoryToFonts, "percent"), fontColor));
            this.CharacterAtlus.Add('.', new TexturedCharacter('.', Path.Combine(directoryToFonts, "period"), fontColor));
            this.CharacterAtlus.Add('+', new TexturedCharacter('+', Path.Combine(directoryToFonts, "plus"), fontColor));
            this.CharacterAtlus.Add('#', new TexturedCharacter('#', Path.Combine(directoryToFonts, "pound"), fontColor));

            this.CharacterAtlus.Add('?', new TexturedCharacter('?', Path.Combine(directoryToFonts, "questionMark"), fontColor));
            this.CharacterAtlus.Add(']', new TexturedCharacter(']', Path.Combine(directoryToFonts, "rightBracket"), fontColor));
            this.CharacterAtlus.Add('}', new TexturedCharacter('}', Path.Combine(directoryToFonts, "rightCurlyBracket"), fontColor));

            this.CharacterAtlus.Add(')', new TexturedCharacter(')', Path.Combine(directoryToFonts, "rightParenthesis"), fontColor));

            this.CharacterAtlus.Add(';', new TexturedCharacter(';', Path.Combine(directoryToFonts, "semicolon"), fontColor));

            this.CharacterAtlus.Add('\'', new TexturedCharacter('\'', Path.Combine(directoryToFonts, "singleQuote"), fontColor));
            this.CharacterAtlus.Add(' ', new TexturedCharacter(' ', Path.Combine(directoryToFonts, "space"), fontColor));
            this.CharacterAtlus.Add('~', new TexturedCharacter('~', Path.Combine(directoryToFonts, "tilde"), fontColor));
            this.CharacterAtlus.Add('_', new TexturedCharacter('_', Path.Combine(directoryToFonts, "underScore"), fontColor));
            this.CharacterAtlus.Add('|', new TexturedCharacter('|', Path.Combine(directoryToFonts, "verticalLine"), fontColor));

            this.CharacterAtlus.Add('$', new TexturedCharacter('$', Path.Combine(directoryToFonts, "coin"), fontColor));
            this.CharacterAtlus.Add('=', new TexturedCharacter('=', Path.Combine(directoryToFonts, "star"), fontColor));
            this.CharacterAtlus.Add('@', new TexturedCharacter('@', Path.Combine(directoryToFonts, "heart"), fontColor));
        }

        public override TexturedCharacter getTexturedCharacter(char c)
        {
            var original = this.CharacterAtlus[c];
            return TexturedCharacter.Copy(original);
        }
    }
}
