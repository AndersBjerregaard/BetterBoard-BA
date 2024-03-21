use std::{error::Error, process::Command};

use chrono::NaiveDate;

pub fn get() -> Result<Vec<(NaiveDate, f32)>, Box<dyn Error>> {
    let command = Command::new("pwsh")
        .arg("../gh-graphql-api/get_project_insights.ps1")
        .output()
        .expect("failed to execute process");

    let output = String::from_utf8_lossy(&command.stdout);

    let mut start_amount: f32 = 320.0_f32;

    let mut values: Vec<(NaiveDate, f32)> = Vec::new();
    let mut lines = output.lines();
    while let Some(value_str) = lines.next() {
        let date_str = lines.next().unwrap().replace("\"", "");
        start_amount -= value_str.trim().parse::<f32>()?;
        let value = start_amount;
        let date = NaiveDate::parse_from_str(&date_str, "%Y-%m-%d")?;
        values.push((date, value));
    }

    println!("{:?}", values);

    Ok(values)
}