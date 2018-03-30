﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.CommandLine.Internal.Parsing.Tokenization
{
    internal sealed class CommandTreeTokenStream : IReadOnlyList<CommandTreeToken>
    {
        private readonly List<CommandTreeToken> _tokens;
        private int _position;

        public int Count => _tokens.Count;

        public CommandTreeToken this[int index] => _tokens[index];

        public CommandTreeToken Current
        {
            get
            {
                if (_position >= Count)
                {
                    return null;
                }
                return _tokens[_position];
            }
        }

        public CommandTreeTokenStream(IEnumerable<CommandTreeToken> tokens)
        {
            _tokens = new List<CommandTreeToken>(tokens ?? Enumerable.Empty<CommandTreeToken>());
            _position = 0;
        }

        public CommandTreeToken Peek()
        {
            return Peek(0);
        }

        public CommandTreeToken Peek(int index)
        {
            var position = _position + index;
            if (position >= Count)
            {
                return null;
            }
            return _tokens[position];
        }

        public CommandTreeToken Consume(CommandTreeToken.Kind type)
        {
            Expect(type);
            if (_position >= Count)
            {
                return null;
            }
            var token = _tokens[_position];
            _position++;
            return token;
        }

        public CommandTreeToken Expect(CommandTreeToken.Kind expected)
        {
            if (Current == null)
            {
                throw ExceptionHelper.Tree.ExpectedTokenButFoundNull(expected);
            }

            var found = Current.TokenKind;
            if (expected != found)
            {
                throw ExceptionHelper.Tree.ExpectedTokenButFoundOther(expected, found);
            }

            return Current;
        }

        public IEnumerator<CommandTreeToken> GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}