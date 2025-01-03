namespace BlockchainTestApp
{
    public static class TestUtils
    {
        private static bool _metaDataLoaded;
        private static Dictionary<string, string> TestDescriptions = new Dictionary<string, string>();

        public static string GetDescription(string testName)
        {
            if (!_metaDataLoaded)
                LoadMetaData();

            if (TestDescriptions.ContainsKey(testName))
                return TestDescriptions[testName];

            return string.Empty;
        }

        private static void LoadMetaData()
        {
            // ToDo: Get the TestMeta.xml file and parse

            _metaDataLoaded = true;
        }
    }
}
