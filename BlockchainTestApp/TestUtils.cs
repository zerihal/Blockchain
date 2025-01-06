namespace BlockchainTestApp
{
    /// <summary>
    /// Placeholder - this class is not yet implemented, however is intended to be used to be able to load
    /// tests from TestMeta.xml files, which can then be run as custom tests in the console.
    /// </summary>
    public static class TestUtils
    {
        private static bool _metaDataLoaded;
        private static Dictionary<string, string> TestDescriptions = new Dictionary<string, string>();

        /// <summary>
        /// Gets test description as loaded from a TestMeta.xml file.
        /// </summary>
        /// <param name="testName">Test name.</param>
        /// <returns>Test description data.</returns>
        public static string GetDescription(string testName)
        {
            if (!_metaDataLoaded)
                LoadMetaData();

            if (TestDescriptions.ContainsKey(testName))
                return TestDescriptions[testName];

            return string.Empty;
        }

        /// <summary>
        /// Load metadata from test file.
        /// </summary>
        private static void LoadMetaData()
        {
            // ToDo: Get the TestMeta.xml file and parse

            _metaDataLoaded = true;
        }
    }
}
