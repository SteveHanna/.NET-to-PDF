using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.IO;
using iTextSharp.text.pdf;

namespace Net2Pdf
{
    public class PdfPrinter
    {
        private StringFormatterCollection _formatters = new StringFormatterCollection();

        public PdfPrinter PerformCustomFormatting<T>(params Expression<Func<T, string>>[] e)
        {
            foreach (var item in e)
                _formatters.Add(new ExpressionFormatter<T>(item));
        
            return this;
        }

        public PdfPrinter ThenPerform<T>(params Expression<Func<T, string>>[] e)
        {
            return PerformCustomFormatting<T>(e);
        }

        public void PrintToPdf(Stream inpStream, Stream outStream, object objToMap)
        {
            if (inpStream == null || outStream == null || objToMap == null)
                throw new ArgumentNullException();

            PdfReader reader = new PdfReader(inpStream);

            using (PdfStamper stamper = new PdfStamper(reader, outStream))
            {
                AcroFields fields = stamper.AcroFields;

                Dictionary<string, string> mappedFields = new PropertyMapper(_formatters ?? new StringFormatterCollection()).GetMappings(objToMap);

                foreach (var item in mappedFields)
                    fields.SetField(item.Key, item.Value);

                stamper.FormFlattening = true;
                stamper.Close();
            }

            reader.Close();
        }
    }
}
