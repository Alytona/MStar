using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sparse_clr_test
{
    class Program
    {
        static void printUsage ()
        {
            Console.WriteLine( "Usage: sparse_clr_test <filename>" );
            //Console.WriteLine( "/t sparse_clr_test img2simg <sparse_image_files> <raw_image_file>" );
            //Console.WriteLine( "/t sparse_clr_test simg2img <sparse_image_files> <raw_image_file>" );
            //Console.WriteLine( "/t sparse_clr_test simg2simg <sparse_image_files> <raw_image_file>" );
        }

        static void Main (string[] args)
        {
            if( args.Length < 1 )
            {
                printUsage();
            }
            else
            {
                try
                {
                    if( !Compress.Sparse.Compress( args[0], 1024, 100*1024*1024 ) )
                    {
                        Console.WriteLine( "Compressing failed" );
                    }
                    else
                    {
                        string[] chunkFiles = Directory.GetFiles( ".", args[0] + ".sparse_chunk.*" );
                        if( !Compress.Sparse.Decompress( chunkFiles, args[0] + ".new" ) )
                            Console.WriteLine( "Decompressing failed" );
                    }
                }
                catch( Exception error ) 
                {
                    Console.WriteLine( error.ToString() );
                }
            }
            Console.WriteLine( "Press any key." );
            Console.ReadKey( true );
        }
    }
}
