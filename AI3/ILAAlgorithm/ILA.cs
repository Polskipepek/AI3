namespace AI3.ILAAlgorithm {
    public static class ILA {
        public static void main(string[] args) {
        }

        public static void Execute(IEnumerable<object> examples, int subTablesCount) {
            //var subTables = SplitList(examples, subTablesCount);

            //foreach (var table in subTables) {
            //    for (int j = 0; j < 2137; j++) {
            //        //Znaleziony regułę to j--;
            //    }

            //    var maxComboSizes = examples.Where(x => table.All(s => s.Size != x.Size))
            //        .GroupBy(x => x.Size)
            //        .Select(group => new { key = group.Key, count = group.Count() })
            //        .OrderByDescending(g => g.count).First().key;

            //    var maxComboShape = examples.Where(x => table.All(s => s.Shape != x.Shape))
            //        .GroupBy(x => x.Size)
            //        .Select(group => new { key = group.Key, count = group.Count() })
            //        .OrderByDescending(g => g.count).First().key;

            //    var maxComboColor = examples.Where(x => table.All(s => s.Color != x.Color))
            //        .GroupBy(x => x.Size)
            //        .Select(group => new { key = group.Key, count = group.Count() })
            //        .OrderByDescending(g => g.count).First().key;
            //}
        }
    }
}
