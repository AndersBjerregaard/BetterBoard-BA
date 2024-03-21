pub mod csv_reader;
pub mod insights;

use chrono::NaiveDate;
use plotters::prelude::*;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    // Sample data
    let projected = csv_reader::read_naive_date()?;
    // let actual = vec![
    //     (NaiveDate::from_ymd_opt(2024, 4, 1).unwrap(), 320.0_f32), 
    //     (NaiveDate::from_ymd_opt(2024, 4, 30).unwrap(), 170.7_f32), 
    //     (NaiveDate::from_ymd_opt(2024, 5, 31).unwrap(), 30.0_f32)];
    let actual = insights::get()?;

    // Set up the chart area
    let root = 
        BitMapBackend::new("plot.png", (1000, 600)).into_drawing_area();

    root.fill(&WHITE)?;

    let mut chart = ChartBuilder::on(&root)
        .caption("Burndown Chart", ("sans-serif", 50).into_font())
        .margin(5)
        .x_label_area_size(30)
        .y_label_area_size(30)
        .build_cartesian_2d(
            NaiveDate::from_ymd_opt(2024, 4, 1).unwrap()
                ..NaiveDate::from_ymd_opt(2024, 6, 2).unwrap(), 
            0.0_f32..320.0_f32)?;

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