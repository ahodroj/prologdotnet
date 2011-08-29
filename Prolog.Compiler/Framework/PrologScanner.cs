//------------------------------------------------------------------------------
// <copyright file="PrologScanner.cs" company="Axiom">
//     
//      Copyright (c) 2006 Ali Hodroj.  All rights reserved.
//     
//      The use and distribution terms for this source code are contained in the file
//      named license.txt, which can be found in the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by the
//      terms of this license.
//     
//      You must not remove this notice, or any other, from this software.
//     
// </copyright>                                                                
//------------------------------------------------------------------------------

namespace Axiom.Compiler.Framework
{	
	using System;
	using System.IO;

	public class PrologScanner
	{
		const char EOF = '\u0080';  // return this for end of file
		const char CR = '\r';
		const char LF = '\n';

		static TextReader _input;
		static TextWriter _output;
	
		static char _ch;            // lookahead character (= next (unprocessed) character in input stream)
		static int _line, _col;      // line and column number of ch in input stream

		private PrologToken _current = null;
		private PrologToken _lookahead;

		private bool _peeked = false;

		public PrologScanner() 
		{
			_input = Console.In;
			_output = Console.Out;
			_line = 1; _col = 0;
			// read 1st char into ch, increment col to 1
			NextCharacter();  
		}

		public PrologScanner(TextReader input) 
		{
			_input = input;
			_output = Console.Out;
			_line = 1; _col = 0;
			// read 1st char into ch, increment col to 1
			NextCharacter();  
		}

		public PrologScanner(TextReader input, TextWriter output) 
		{
			_input = input;
			_output = output;
			_line = 1; _col = 0;
			// read 1st char into ch, increment col to 1
			NextCharacter();  
		}

		/* Reads next character from input stream into ch.
		* Keeps pos, line and col in sync with reading position. */
		private void NextCharacter() 
		{
			try 
			{
				_ch = (char) _input.Read();
				switch (_ch) 
				{
					case CR: 
						_ch = (char) _input.Read();  // skip CR
						_line++; _col = 0;
						break;
					case LF:
						_line++; _col = 0; break;
					case '\uffff':  // read returns -1 at end of file
						_ch = EOF; break;
					default: _col++; break;
				}
			} 
			catch (IOException) 
			{
				_ch = EOF;
			}	
		}


		/// <summary>
		/// Returns the next token to be used by the parser.
		/// </summary>
		/// <returns></returns>
		public PrologToken Next() 
		{
			if(_peeked == true) 
			{
				_current = _lookahead;
				_peeked = false;
				return _current;
			}
			PrologToken tok = new PrologToken(PrologToken.EOF, _line, _col);
			string val = "";	
			while(_ch != EOF) 
			{
				/* comment encountered */
				if(_ch == '%') 
				{
					while(_input.Read() != '\n') 
					{

					}
					NextCharacter();
					continue;
				}

                if (_ch == '/')
                {
                    _ch = (char)_input.Peek();
                    if (_ch == '*')
                    {
                        // Comment started
                        _input.Read();
                        while (true)
                        {
                            _ch = (char)_input.Peek();
                            if (_ch == '*')
                            {
                                _input.Read();
                                _ch = (char)_input.Peek();
                                if (_ch == '/')
                                {
                                    _input.Read();
                                    NextCharacter();
                                    break;
                                }
                                else
                                {
                                    //_input.Read();
                                }
                            }
                            else
                            {
                                _input.Read();
                            }
                        }
                    }
                    else
                    {
                        _ch = '/';
                    }
                }

                
				if(_ch == '_') 
				{
					tok.Kind = PrologToken.ATOM;
					tok.StringValue = _ch.ToString();
					_current = tok;
					NextCharacter();
					return tok;
				}
				/* Recognizes: numbers */
				if(Char.IsDigit(_ch)) 
				{
					tok.Kind = PrologToken.ATOM;
				
					while(Char.IsDigit(_ch)) 
					{
						val += _ch.ToString();
						NextCharacter();
					}
					tok.IntValue = Int32.Parse(val);
					tok.StringValue = val;
					_current = tok;
					return tok;
				}
					/* Recognizes: identifiers, not, mod */
				else if(Char.IsLetter(_ch)) 
				{
					tok.Kind = PrologToken.ATOM;
					while(Char.IsLetterOrDigit(_ch) || _ch == '_') 
					{
						val += _ch.ToString();
						NextCharacter();
					}
					if(Char.IsUpper(val, 0) || val[0] == '_' || val == "_") 
					{
						tok.Kind = PrologToken.VARIABLE;

					}
					tok.StringValue = val;
					_current = tok;
					return tok;
				}
				else if(Char.IsPunctuation(_ch) || Char.IsSymbol(_ch)) 
				{

                    
					tok.Kind = PrologToken.ATOM;
					val += _ch.ToString();
					switch(val) 
					{
                        
						case ",":
							tok.Kind = PrologToken.COMMA;
							NextCharacter();
							break;
						case "|":
							tok.Kind = PrologToken.LIST_SEP;
							NextCharacter();
							break;
						case "(":
							tok.Kind = PrologToken.LPAREN;
							NextCharacter();
							break;
						case ")":
							tok.Kind = PrologToken.RPAREN;
							NextCharacter();
							break;
						case "[":
							tok.Kind = PrologToken.LBRACKET;
							NextCharacter();
							break;
						case "]":
							tok.Kind = PrologToken.RBRACKET;
							NextCharacter();
							break;
						case "!":
							tok.Kind = PrologToken.STRING;
							NextCharacter();
							break;
						case ".":
							tok.Kind = PrologToken.DOT;
							NextCharacter();
							break;
						case "'":
							tok.Kind = PrologToken.STRING;
							NextCharacter();
							while(_ch != '\'') 
							{
								val += _ch.ToString();
								NextCharacter();
							}
							NextCharacter();
							//val = val.Remove(0,1);
							val += "'";
							tok.StringValue = val;
							_current = tok;
							break;
                        case "\"":
                            tok.Kind = PrologToken.STRING;
                            NextCharacter();
                            while (_ch != '"')
                            {
                                val += _ch.ToString();
                                NextCharacter();
                            }
                            NextCharacter();
                            //val = val.Remove(0,1);
                            val += "\"";
                            tok.StringValue = val;
                            _current = tok;
                            break;
                        //default:
                        //    Error("Invalid token: " + val);
                        //    break;

					}
					if(tok.Kind != PrologToken.ATOM) 
					{
						tok.StringValue = val;
						if(tok.Kind == PrologToken.STRING) 
							tok.Kind = PrologToken.ATOM;
						_current = tok;
						return tok;
					}
					val = "";
					while((Char.IsPunctuation(_ch) || Char.IsSymbol(_ch)) && _ch != '(') 
					{
						val += _ch.ToString();
						NextCharacter();
					}
					tok.StringValue = val;
					_current = tok;
					return tok;
				}
				NextCharacter();
			}
			_current = tok;
			return tok;
		}

		/* Error handling for the scanner.
			* Prints error message to output. */
		private void Error (string msg) 
		{
			// leave this here for scanner testing
			if (_output != null) _output.WriteLine("-- line {0}, col {1}: {2}", _line, _col, msg);
		}

		/// <summary>
		/// Gets or sets the current scanner token.
		/// </summary>
		public PrologToken Current 
		{
			get { return this._current; }
			set { this._current = value; }
		}

		/// <summary>
		/// Gets or sets the current look-ahead token.
		/// </summary>
		public PrologToken Lookahead
		{
			get 
			{
				/* Save current token */
				if(_current != null) 
				{
					PrologToken saveCurrent = new PrologToken(_current.Kind, _current.Line, _current.Column, _current.IntValue, _current.StringValue);
					_lookahead = this.Next();
					this._peeked = true;
					this._current = saveCurrent;
				} 
				else 
				{
					_lookahead = this.Next();
					_current = null;
					this._peeked = true;
				}
				return this._lookahead;
			}
		}
	
	}
}