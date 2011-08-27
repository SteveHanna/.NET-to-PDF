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
        private bool _hasAdded = false;

        public PdfPrinter AddCustomFormat<T>(params Expression<Func<T, string>>[] e)
        {
            if (_hasAdded)
                throw new InvalidOperationException("Items have already been added, to add more items please use the \"ThenAdd\" method");

            foreach (var item in e)
                _formatters.Add(new ExpressionFormatter<T>(item));

            return this;
        }

        public PdfPrinter ThenAdd<T>(params Expression<Func<T, string>>[] e)
        {
            foreach (var item in e)
                _formatters.Add(new ExpressionFormatter<T>(item));

            return this;
        }

        public void PrintToPdf(Stream inputFileName, Stream outStream, object objToMap)
        {
            if (inputFileName == null || outStream == null || objToMap == null)
                throw new ArgumentNullException();

            PdfReader reader = new PdfReader(inputFileName);

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
