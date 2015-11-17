using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrankCompiler.Core
{
    public class FormulasBienFormadasParser : AbstractParser
    {
        public FormulasBienFormadasParser()
        {
            Scaner = new Scaner();
        }

        bool EsVariable(Token token)
        {

            return !string.IsNullOrEmpty(token.Lexem) && token.Lexem[0] != token.LexemLower[0];
        }
        protected override void EntryPoint()
        {
            NextToken();
            Expresion();
            if (t.Type != TokenType.NoToken)
                AddError("No se esperaban mas tokens", t);
        }

        void Expresion()
        {
            if (t == null)
            {
                idx--;
                NextToken();
                AddError("Se esperaba una expresión", t);
            }
            else if (t.Type == TokenType.NoToken)
            {
                AddError("Se esperaba una expresión", t);
            }
            else
            {

                if (t.LexemLower == "(")
                {
                    NextToken();
                    Expresion();
                    if (t.LexemLower != ")")
                    {
                        AddError("Se esperaba paréntesis ')'", t);
                    }
                    NextToken();
                }
                else if (t.LexemLower == "~")
                {
                    ExpresionUnaria();
                }
                else if (t.Type == TokenType.Cuantif)
                {
                    Cuantificacion();
                    Expresion();
                }
                else if (t.Type == TokenType.Identi)
                {
                    Atomo();
                }
                else
                {
                    AddError("Se esperaba un identificador", t);
                }

                if (t.Type == TokenType.OpLog)
                {
                    NextToken();
                    Expresion();
                }
                else if (t.Type == TokenType.OpRela)
                {
                    NextToken();
                    Expresion();
                }
            }
        }

        void ExpresionUnaria()
        {
            NextToken();
            Expresion();
        }

        void Cuantificacion()
        {
            NextToken();
            Var();
        }

        void Var()
        {
            if (t.Type != TokenType.Identi || !EsVariable(t))
            {
                AddError("Se esperaba variable", t);
            }
            NextToken();
        }

        void Constante()
        {
            if (t.Type != TokenType.Identi || EsVariable(t))
            {
                AddError("Se esperaba un termino", t);
            }
            NextToken();
        }

        void Funcion()
        {
            Constante();
            if (t.LexemLower != "(")
                AddError("Se esperaba parentesis '('", t);
            NextToken();
            ListaTerminos();
            if (t.LexemLower != ")")
                AddError("Se esperaba parentesis ')'", t);
            NextToken();
        }

        void Termino()
        {
            if (EsVariable(t)) Var();
            else
            {
                Constante();
                if (t.LexemLower == "(")
                {
                    idx--;
                    Funcion();
                }
            }
        }

        void ListaTerminos()
        {
            do
            {
                Termino();
            } while (t.Lexem == "," && NextToken() != null);
        }
        void Comparacion()
        {
            if (t.Type != TokenType.OpRela)
            {
                AddError("Se esperaba operador de comparacion", t);
            }
            NextToken();
            Termino();
        }
        void Atomo()
        {
            if (EsVariable(t)) {
                NextToken();
                if(t.Type == TokenType.OpRela)
                    Comparacion();
            }
            else if (t.Type == TokenType.Identi)
            {
                NextToken();
                if (t.Lexem == "(")
                {
                    idx -= 2;
                    NextToken();
                    Funcion();
                }
                else
                {
                    //if (t.Type != TokenType.OpRela)
                    //{
                    //    AddError("Se esperaba paréntesis u operador de comparacion", t);
                    //}
                    //idx -= 2;
                    //NextToken();
                    Comparacion();
                }
            }
        }


    }
}
