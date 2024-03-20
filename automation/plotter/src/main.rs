use std::{error::Error, fs::File};

use plotters::prelude::*;
use csv::{self, ReaderBuilder};

fn main() -> Result<(), Box<dyn std::error::Error>> {
    // Sample data
    let projected = projected()?;
    let actual = vec![(0, 320.0), (30, 170.7), (61, 30.0)];

    // Set up the chart area
    let root = 
        BitMapBackend::new("plot.png", (1000, 600)).into_drawing_area();

    root.fill(&WHITE)?;

    let mut chart = ChartBuilder::on(&root)
        .caption("Burndown Chart", ("sans-serif", 50).into_font())
        .margin(5)
        .x_label_area_size(30)
        .y_label_area_size(30)
        .build_cartesian_2d(0..61, 0.0..320.0)?;

    chart.configure_mesh().draw()?;

    chart.draw_series(LineSeries::new(
        projected,
        &RED,
        ))?
        .label("projected")
        .legend(|(x, y)| PathElement::new(vec![(x,y), (x + 20, y)], &RED));

    chart.draw_series(LineSeries::new(
        actual,
        &BLUE,
        ))?
        .label("actual")
        .legend(|(x, y)| PathElement::new(vec![(x,y), (x + 20, y)], &BLUE));

    chart.configure_series_labels()
        .background_style(&WHITE.mix(0.8))
        .border_style(&BLACK)
        .draw()?;

    Ok(())
}

fn projected() -> Result<Vec<(i32, f64)>, Box<dyn Error>> {
    let mut projected = Vec::new();
    let file = File::open("projected.csv")?;
    let mut rdr = ReaderBuilder::new().has_headers(false).from_reader(file);
    for result in rdr.records() {
        let record = result?;
        let x: i32 = record[0].parse()?;
        let y: f64 = record[1].parse()?;
        projected.push((x, y));
    }
    Result::Ok(projected)
}