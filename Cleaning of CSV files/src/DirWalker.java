import java.io.File;
import java.util.ArrayList;
import java.util.logging.Level;
import java.util.logging.Logger;

public class DirWalker {
	public static int skippedRow = 0, validRow = 0, totalValidRow = 0, totalSkippedRow = 0;
	public static ArrayList<String> csvFiles = new ArrayList<String>();
	public void walk(String path) {
		try {
		File root = new File(path);
		File[] list = root.listFiles();
		
		if (list == null)
			return;
		
		for (File f : list) {
			if (f.isDirectory()) {
				walk(f.getAbsolutePath());
			} else {
				String fp = f.getAbsolutePath();
				if (fp.endsWith(".csv") || fp != null) {
					csvFiles.add(fp);

				}
			}
		}
		}
		catch(Exception e) {
			
		}
	}


	public static void main(String[] args) {
		final long startTime = System.currentTimeMillis();
		DirWalker fw = new DirWalker();
		SimpleLogging logNow = new SimpleLogging();
		logNow.logForME();
		Logger logger = Logger.getLogger("Main");
		logger.log(Level.INFO, "This is a info log message ");
		fw.walk("C:\\Users\\gadhi\\Documents\\GitHub\\MCDA5510_Assignments\\Sample-Data\\Sample-Data");
		SimpleCsvParser.readFile();
		logger.log(Level.INFO, "Data Skipped Row: " + totalSkippedRow + " totalValid Row: " + totalValidRow);
		final long endTime = System.currentTimeMillis();
		logger.log(Level.INFO,"Total execution time: " + (endTime - startTime));
	}

}
