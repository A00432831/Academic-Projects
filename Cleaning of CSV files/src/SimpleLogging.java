import java.io.IOException;
import java.util.logging.ConsoleHandler;
import java.util.logging.FileHandler;
import java.util.logging.Formatter;
import java.util.logging.Handler;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.logging.SimpleFormatter;

public class SimpleLogging {

	public void logForME() {

		Handler consoleHandler = null;

		Handler fileHandler = null;
		Formatter simpleFormatter = null;

		Logger logger = Logger.getLogger("Main");


		// Creating consoleHandler and fileHandler
		consoleHandler = new ConsoleHandler();
		try {
			fileHandler = new FileHandler("C:\\Users\\gadhi\\Documents\\GitHub\\MCDA5510_Assignments\\Assignment1\\Logs\\LogFile.log");
		} catch (SecurityException | IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		// Assigning handlers to LOGGER object
		logger.addHandler(consoleHandler);
		logger.addHandler(fileHandler);
		// Setting levels to handlers and LOGGER
		consoleHandler.setLevel(Level.ALL);
		fileHandler.setLevel(Level.FINE);
		logger.setLevel(Level.ALL);
		
		simpleFormatter = new SimpleFormatter();
		
		// Setting formatter to the handler
		fileHandler.setFormatter(simpleFormatter);

	}

}
