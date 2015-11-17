using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FrankCompiler.Core
{
    public class Scaner : IScaner
    {
        #region const values
        const int ERR = -1, ACP = 999;


        static readonly int[,] matran = {
    	    //		letra digito del    oprel   -     >     <    =     !      Cuant  _           
    	    /*0*/  {13,   ERR,   8,     1,      3,    11 ,  5  , 9  ,  10 ,   2  ,   ERR, 0   },
    	    /*1*/  {ACP,  ACP,   ACP,   ACP,    ACP,  ACP,  ACP, ACP,  ACP,   ACP,   ACP, ACP }, //op log		
    	    /*2*/  {ACP,  ACP,   ACP,   ACP,    ACP,  ACP,  ACP, ACP,  ACP,   ACP,   ACP, ACP }, //cuantif
    	    /*3*/  {ERR,  ERR,   ERR,   ERR,    ERR,  4  ,  ERR, ERR,  ERR,   ERR,   ERR, ERR }, 
    	    /*4*/  {ACP,  ACP,   ACP,   ACP,    ACP,  ACP,  ACP, ACP,  ACP,   ACP,   ACP, ACP }, //op log
    	    /*5*/  {ACP,  ACP,   ACP,   ACP,    6  ,  ACP,  ACP, 12 ,  ACP,   ACP,   ACP, ACP }, //op comp
    	    /*6*/  {ACP,  ACP,   ACP,   ACP,    ACP,  7  ,  ACP, ACP,  ACP,   ACP,   ACP, ACP }, //op log  
    	    /*7*/  {ACP,  ACP,   ACP,   ACP,    ACP,  ACP,  ACP, ACP,  ACP,   ACP,   ACP, ACP }, //op log
    	    /*8*/  {ACP,  ACP,   ACP,   ACP,    ACP,  ACP,  ACP, ACP,  ACP,   ACP,   ACP, ACP }, //delim
    	    /*9*/  {ACP,  ACP,   ACP,   ACP,    ACP,  ACP,  ACP, ACP,  ACP,   ACP,   ACP, ACP }, //op comp
           /*10*/  {ERR,  ERR,   ERR,   ERR,    ERR,  ERR,  ERR, 9  ,  ERR,   ERR,   ERR, ERR },
           /*11*/  {ACP,  ACP,   ACP,   ACP,    ACP,  ACP,  ACP, 12 ,  ACP,   ACP,   ACP, ACP }, //Op comp
           /*12*/  {ACP,  ACP,   ACP,   ACP,    ACP,  ACP,  ACP, ACP,  ACP,   ACP,   ACP, ACP }, //Op comp
           /*13*/  {13 ,  13 ,   ACP,   ACP,    ACP,  ACP,  ACP, ACP,  ACP,   ACP,   13 , ACP }, //Op rela
	    };
        public static string[] reservadas = {
		    /*"para", "mientras", "en", "inicio", "fin", "regresa", "si", "sino", "seleccion",
		    "caso", "interrumpe", "otro", "lee", "imprime", "imprimenl", "constante", 
		    "principal", "y","o","no"*/
	    };
        #endregion

        string entrada = "";
        int idx = 0;
        ScanResult sr { get; set; }
        public static bool palRes(string p)
        {
            return reservadas.Contains(p.ToLower());
        }
        public static int colCar(char c)
        {
            if (char.IsLetter(c)) return 0;
            if (char.IsDigit(c)) return 1;
            if (c == '(' || c == ')' || c == ',') return 2;
            if (c == '&' || c == '|' || c == '~') return 3;
            if (c == '-') return 4;
            if (c == '>') return 5;
            if (c == '<') return 6;
            if (c == '=') return 7;
            if (c == '!') return 8;
            if (c == '#' || c == '@') return 9;
            if (c == '_') return 10;
            if( c == ' ' || c == '\t' ) return 11;
            return ERR;
        }

        void AddError(string error, int line = 0, int column=0)
        {
            sr.Errors.Add(new CompilerError() { Line = line, Message = error, Type = ErrorType.Lexicographic, Column=column });
        }

        public Token nextToken(int line = 0)
        {
            int estado = 0, estAnt = 0;
            string lexema = "";
            TokenType token;
            while (idx < entrada.Length && estado != ERR && estado != ACP)
            {
                char c = entrada[idx++];
                while (estado == 0 && (c == ' ' || c == '\t')) c = entrada[idx++];
                int col = colCar(c);
                if (col >= 0)
                {
                    estado = matran[estado, col];
                    if (estado != ERR && estado != ACP)
                    {
                        estAnt = estado;
                        lexema += c;
                    }
                }
                else estado = ERR;

                if (estado == ERR)
                    AddError("Simbolo ilegal en el lenguaje " + c, line,idx);
            }
            if (estado == ACP) idx--;
            if (estado != ERR && estado != ACP) estAnt = estado;
            token = TokenType.NoToken;
            switch (estAnt)
            {
                case 1:
                case 4:
                case 6:
                case 7:
                    token = TokenType.OpLog; break;
                case 2:
                    token = TokenType.Cuantif; break;
                case 5:
                case 9:
                case 11:
                case 12:
                    token = TokenType.OpRela; break;
                case 8:
                    token = TokenType.Delimi; break;
                case 13:
                    token = TokenType.Identi; break;
            }
            var t = new Token(token, lexema);
            t.Line = line + 1;
            t.Column = idx;
            return t;
        }
        public ScanResult Scan(string input)
        {
            sr = new ScanResult();
            entrada = input.TrimEnd();
            idx = 0;
            while (idx < entrada.Length)
            {
                try
                {
                    var t = nextToken();
                    if (t != null)
                        sr.Tokens.Add(t);
                }
                catch (Exception ex)
                {
                    AddError("Error in parser: " + ex.Message);
                }
            }
            return sr;

        }

        public ScanResult Scan(string[] input)
        {
            sr = new ScanResult();
            for (int i = 0; i < input.Length; i++)
            {
                entrada = input[i].TrimEnd();
                idx = 0;
                while (idx < entrada.Length)
                {
                    try
                    {
                        var t = nextToken(i);
                        if (t != null && t.Type != TokenType.NoToken)
                            sr.Tokens.Add(t);
                        else
                            break;
                    }
                    catch (Exception ex)
                    {
                        AddError("Error in scanner: " + ex.Message, i);
                    }
                }
            }
            return sr;
        }

        public ScanResult ScanFromFile(string filename)
        {
            sr = null;
            if (!File.Exists(filename))
            {
                sr = new ScanResult();
                AddError("El archivo no existe", -1);
                return sr;
            }
            var input = File.ReadAllLines(filename);
            sr = Scan(input);
            return sr;
        }

    }
}
