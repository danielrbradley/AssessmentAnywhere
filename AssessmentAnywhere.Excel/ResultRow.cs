namespace AssessmentAnywhere.Excel
{
    public class ResultRow
    {
        private readonly string surname;

        private readonly string forenames;

        private readonly decimal? result;

        public ResultRow(string surname, string forenames, decimal? result)
        {
            this.surname = surname;
            this.forenames = forenames;
            this.result = result;
        }

        public string Surname
        {
            get
            {
                return this.surname;
            }
        }

        public string Forenames
        {
            get
            {
                return this.forenames;
            }
        }

        public decimal? Result
        {
            get
            {
                return this.result;
            }
        }
    }
}