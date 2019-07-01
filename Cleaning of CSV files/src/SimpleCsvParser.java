
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.Reader;
import java.util.logging.Level;
import java.util.logging.Logger;

import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVRecord;

public class SimpleCsvParser extends DirWalker {
	public static void readFile() {
		Logger logger = Logger.getLogger("Main");
		Reader in;
	
		String pathValid = "C:\\Users\\gadhi\\Documents\\GitHub\\MCDA5510_Assignments\\Assignment1\\Output\\ValidRow1.csv";
		try{
			PrintWriter writer = new PrintWriter(new File(pathValid));
			String headers = "First Name,Last Name,StreetNo,Street,City,Province,Postal Code,Country,Phone Number,email address,Date,\n";
			writer.write(headers);
			for (String f : csvFiles) {
				
			in = new FileReader(f);
			Iterable<CSVRecord> records = CSVFormat.EXCEL.withHeader("First Name", "Last Name", "StreetNo", "Street","City", "Province", "Postal Code", "Country", "Phone Number", "email address","Date").parse(in);
			validRow = 0;
			skippedRow = 0;
			String date[]=f.split("\\\\");
			boolean skipHeader = true;
			for (CSVRecord record : records) {
				StringBuilder builder = new StringBuilder();
				if (skipHeader) {
					skipHeader = false;
					continue;
				}
				try {
					String fname = record.get("First Name");
					String lname = record.get("Last Name");
					String streetNo = record.get("StreetNo");
					String street = record.get("Street");
					String city = record.get("City");
					String province = record.get("Province");
					String postalCode = record.get("Postal Code");
					String country = record.get("Country");
					String phone = record.get("Phone Number");
					String email = record.get("email address");
					
					boolean checkSkipRow = false;

					for (int i = 0; i <= 9; i++) {
						if (record.get(i).isEmpty()) {
							checkSkipRow = true;
							break;
						}
					}

					if (checkSkipRow) {
						skippedRow++;
						builder.append(fname);
						builder.append(",");
						builder.append(lname);
						builder.append(",");
						builder.append(streetNo);
						builder.append(",");
						builder.append(street);
						builder.append(",");
						builder.append(city);
						builder.append(",");
						builder.append(province);
						builder.append(",");
						builder.append(postalCode);
						builder.append(",");
						builder.append(country);
						builder.append(",");
						builder.append(phone);
						builder.append(",");
						builder.append(email);
						builder.append(",");
						builder.append(date[8]+"-"+date[9]+"-"+date[10]);
						builder.append("\n");
						logger.log(Level.WARNING,"Invalid Row: " + builder.toString());
					} else {
						validRow += 1;
						builder.append(fname);
						builder.append(",");
						builder.append(lname);
						builder.append(",");
						builder.append(streetNo);
						builder.append(",");
						builder.append(street);
						builder.append(",");
						builder.append(city);
						builder.append(",");
						builder.append(province);
						builder.append(",");
						builder.append(postalCode);
						builder.append(",");
						builder.append(country);
						builder.append(",");
						builder.append(phone);
						builder.append(",");
						builder.append(email);
						builder.append(",");
						builder.append(date[8]+"-"+date[9]+"-"+date[10]);
						builder.append("\n");
						writer.write(builder.toString());
					}	
				}

				catch (Exception e) {
					
				}
			}
			totalValidRow = totalValidRow + validRow;
			totalSkippedRow = totalSkippedRow + skippedRow;

			}
			writer.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}


}
