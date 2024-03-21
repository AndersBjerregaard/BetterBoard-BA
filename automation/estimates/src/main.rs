use std::{error::Error, process::Command};
use chrono::NaiveDate;

fn main() -> Result<(), Box<dyn Error>> {
    let command = Command::new("pwsh")
        .arg("../gh-graphql-api/get_project_insights.ps1")
        .output()
        .expect("failed to execute process");

    let output = String::from_utf8_lossy(&command.stdout);

    let mut values: Vec<(NaiveDate, f32)> = Vec::new();
    let mut lines = output.lines();
    while let Some(value_str) = lines.next() {
        let date_str = lines.next().unwrap().replace("\"", "");
        let value = value_str.trim().parse::<f32>()?;
        // let date = NaiveDate::parse_from_str("2024-04-02", "%Y-%m-%d")?;
        let date = NaiveDate::parse_from_str(&date_str, "%Y-%m-%d")?;
        values.push((date, value));
    }

    println!("{:?}", values);

    for x in values {
        println!("{}", x.0);
        println!("{}", x.1);
    }

    Ok(())
}
