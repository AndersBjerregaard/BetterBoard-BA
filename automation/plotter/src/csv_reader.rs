use std::{error::Error, fs::File};
use chrono::NaiveDate;
use csv::ReaderBuilder;

pub fn read_i32() -> Result<Vec<(i32, f32)>, Box<dyn Error>> {
    let mut projected = Vec::new();
    let file = File::open("projected.csv")?;
    let mut rdr = ReaderBuilder::new().has_headers(false).from_reader(file);
    for result in rdr.records() {
        let record = result?;
        let x: i32 = record[0].parse()?;
        let y: f32 = record[1].parse()?;
        projected.push((x, y));
    }
    Result::Ok(projected)
}

pub fn read_naive_date() -> Result<Vec<(NaiveDate, f32)>, Box<dyn Error>> {
    let mut projected = Vec::new();
    let file = File::open("projected2.csv")?;
    let mut rdr = ReaderBuilder::new().has_headers(false).from_reader(file);
    for result in rdr.records() {
        let record = result?;
        // Parse to NaiveDate
        let x_str: String = record[0].parse()?;
        let x = NaiveDate::parse_from_str(&x_str, "%Y-%m-%d")?;
        let y: f32 = record[1].parse()?;
        projected.push((x, y));
    }
    Result::Ok(projected)
}