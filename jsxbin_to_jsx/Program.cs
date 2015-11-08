﻿using Jsbeautifier;
using jsxbin_to_jsx.JsxbinDecoding;
using System;
using System.IO;
using System.Text;

namespace jsxbin_to_jsx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                PrintHelp();
                return;
            }
            var parsedArgs = ParseCommandLine(args);
            if (parsedArgs == null)
            {
                PrintHelp();
                return;
            }
            Decode(parsedArgs);
        }

        static void PrintHelp()
        {
            Console.WriteLine("Invalid arguments given.");
            Console.WriteLine("Usage: jsxbin_to_jsx  --jsxbin <encoded-jsxbin-filepath> --jsx <decoded-jsx-filepath>");
        }

        static void Decode(DecodeArgs decoderArgs)
        {
            Console.WriteLine("Decoding {0}", decoderArgs.JsxbinFilepath);
            string jsxbin = File.ReadAllText(decoderArgs.JsxbinFilepath, Encoding.ASCII);
            string jsx = AbstractNode.Decode(jsxbin);
            // https://github.com/ghost6991/Jsbeautifier
            jsx = new Beautifier().Beautify(jsx, new BeautifierOptions()
            {
                PreserveNewlines = true
            });
            File.WriteAllText(decoderArgs.JsxFilepath, jsx, Encoding.UTF8);
            Console.WriteLine("Jsxbin successfully decoded to {0}", decoderArgs.JsxFilepath);
        }

        static DecodeArgs ParseCommandLine(string[] args)
        {
            var decoderArgs = new DecodeArgs();
            for (int argIndex = 0; argIndex < args.Length - 1; argIndex++)
            {
                if (args[argIndex] == "--jsxbin")
                {
                    decoderArgs.JsxbinFilepath = args[argIndex + 1];
                    argIndex++;
                }
                else if (args[argIndex] == "--jsx")
                {

                    decoderArgs.JsxFilepath = args[argIndex + 1];
                    argIndex++;
                }
                else
                {
                    return null;
                }
            }
            return decoderArgs;
        }

        private class DecodeArgs
        {
            public string JsxFilepath { get; set; }
            public string JsxbinFilepath { get; set; }
        }
    }
}